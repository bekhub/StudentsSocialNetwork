using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Common.Exceptions;
using Common.Extensions;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ObisApiModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Services
{
    public class StudentsService
    {
        private readonly SsnDbContext _context;
        private readonly IObisApiService _obisApiService;
        private readonly ILogger<StudentsService> _logger;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public StudentsService(SsnDbContext ssnDbContext, IObisApiService obisApiService,
            IEncryptionService encryptionService, ILogger<StudentsService> logger, IMapper mapper)
        {
            _context = ssnDbContext;
            _obisApiService = obisApiService;
            _encryptionService = encryptionService;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task SynchronizeStudentCoursesAsync(string studentNumber)
        {
            var student = await _context.Students
                .Include(x => x.StudentCourses)
                .SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
            await SynchronizeStudentCoursesAsync(student);
        }

        public async Task SynchronizeStudentCoursesAsync(Student student)
        {
            if (student == null) throw new NotFoundException("Student not found");
            var stopwatch = new Stopwatch();
            _logger.LogWarning("Start synchronizing ({Firstname} {Lastname}) student's courses",
                student.Firstname, student.Lastname);
            stopwatch.Start();

            await SynchronizeStudentCourses(student);

            stopwatch.Stop();
            _logger.LogWarning(
                "End synchronizing ({Firstname} {Lastname}) student's courses. {Milliseconds} ms elapsed",
                student.Firstname, student.Lastname, stopwatch.Elapsed.Milliseconds);
        }

        private async Task SynchronizeStudentCourses(Student student)
        {
            var studentNumber = student.StudentNumber;
            var studentPassword = await _encryptionService.DecryptAsync(student.StudentPassword);

            var response = await _obisApiService.AuthenticateAsync(studentNumber, studentPassword);
            if (response == null || string.IsNullOrEmpty(response.AuthKey))
                throw new LogicException("Something went wrong :(. Student password can be invalid");

            student.AuthKey = response.AuthKey;
            var takenLessons = await _obisApiService.StudentTakenLessonsAsync();
            var studentCourses = await GetStudentCoursesAsync(takenLessons, student.Id);
            student.StudentCourses = studentCourses;
            await _context.SaveChangesAsync();
        }

        private async Task<List<StudentCourse>> GetStudentCoursesAsync(List<StudentTakenLessons.Response> models,
            int studentId)
        {
            if (models == null || models.Count == 0)
            {
                models = await _context.StudentCourses
                    .Include(x => x.Student)
                    .Include(x => x.Course)
                    .Where(x => x.Student.Id == studentId)
                    .Select(x => _mapper.Map<StudentTakenLessons.Response>(x))
                    .ToListAsync();
            }
            var semesterNotes = await _obisApiService.StudentSemesterNotesAsync();
            var studentCourses = new List<StudentCourse>();
            foreach (var model in models)
            {
                var lesson = semesterNotes.GetLessonByCodeAndName(model.Code, model.Name);
                var assessments = await GetAssessmentsAsync(lesson, studentId);
                var studentCourse = await GetStudentCourseAsync(model, assessments, studentId);
                studentCourse.AverageAssessment = lesson?.Exams.FirstOrDefault(x => !string.IsNullOrEmpty(x.Avg))?.Avg;
                studentCourses.Add(studentCourse);
            }

            return studentCourses;
        }

        private async Task<StudentCourse> GetStudentCourseAsync(StudentTakenLessons.Response model,
            ICollection<Assessment> assessments, int studentId)
        {
            var course = await GetCourseAsync(model);
            var studentCourse = await _context.StudentCourses
                .FirstOrDefaultAsync(x =>
                    x.CourseId == course.Id &&
                    x.StudentId == studentId);

            if (studentCourse != null)
            {
                studentCourse.TheoryAbsent = model.TheoryAbsent.AsIntOrZero();
                studentCourse.PracticeAbsent = model.PracticeAbsent.AsIntOrZero();
                studentCourse.Assessments = assessments;
                return studentCourse;
            }

            return new StudentCourse
            {
                TheoryAbsent = model.TheoryAbsent.AsIntOrZero(),
                PracticeAbsent = model.PracticeAbsent.AsIntOrZero(),
                AcademicYear = StudentCourse.CurrentAcademicYear,
                Semester = StudentCourse.CurrentSemester,
                Course = course,
                Assessments = assessments,
            };
        }

        private async Task<List<Assessment>> GetAssessmentsAsync(StudentSemesterNotes.Lesson model, int studentId)
        {
            if (model is null) return null;

            var studentCourse = await _context.StudentCourses
                .Include(x => x.Course)
                .Include(x => x.Assessments)
                .FirstOrDefaultAsync(x =>
                    x.StudentId == studentId &&
                    x.Course.Code == model.LessonCodeFromName &&
                    x.Course.Name == model.LessonNameFromName);
            if (studentCourse == null || studentCourse.Assessments.Count == 0 || !studentCourse.IsActive)
            {
                return model.Exams.Select(x => new Assessment
                {
                    Type = x.Name,
                    Point = x.Mark.AsIntOrNull(),
                }).ToList();
            }

            var examNames = model.Exams.ToDictionary(x => x.Name);
            
            var assessmentsToUpdate = studentCourse.Assessments
                .Where(x => examNames.ContainsKey(x.Type))
                .Select(x =>
                {
                    x.Point = examNames[x.Type].Mark.AsIntOrNull();
                    return x;
                }).ToDictionary(x => x.Type);
            
            var assessmentsToCreate = examNames.Values
                .Where(x => !assessmentsToUpdate.ContainsKey(x.Name))
                .Select(x => new Assessment
                {
                    Type = x.Name,
                    Point = x.Mark.AsIntOrNull(),
                }).Concat(assessmentsToUpdate.Values);

            return assessmentsToCreate.ToList();
        }

        private async Task<Course> GetCourseAsync(StudentTakenLessons.Response model)
        {
            return await _context.Courses
                       .FirstOrDefaultAsync(x => x.Code == model.Code && x.Name == model.Name) ??
                   new Course
                   {
                       Code = model.Code,
                       Name = model.Name,
                       Credits = model.Credit.AsIntOrZero(),
                       Practice = model.Practice.AsIntOrZero(),
                       Theory = model.Theory.AsIntOrZero(),
                   };
        }
    }
}