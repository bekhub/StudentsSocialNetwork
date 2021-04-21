using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
        private readonly IRestApiService _restApiService;
        private readonly ILogger<StudentsService> _logger;
        private readonly IEncryptionService _encryptionService;

        public StudentsService(SsnDbContext ssnDbContext, IRestApiService restApiService,
            IEncryptionService encryptionService, ILogger<StudentsService> logger)
        {
            _context = ssnDbContext;
            _restApiService = restApiService;
            _encryptionService = encryptionService;
            _logger = logger;
        }

        public async Task LoggedSynchronizeStudentCourses(string studentNumber)
        {
            var student = await _context.Students
                .Include(x => x.StudentCourses)
                .SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
            if (student == null) return;
            var stopwatch = new Stopwatch();
            _logger.LogWarning("Start synchronizing ({Firstname} {Lastname}) student's courses",
                student.Firstname, student.Lastname);
            stopwatch.Start();

            await SynchronizeStudentCourses(student);

            stopwatch.Stop();
            _logger.LogWarning("End synchronizing ({Firstname} {Lastname}) student's courses. {Milliseconds} ms elapsed", 
                student.Firstname, student.Lastname, stopwatch.Elapsed.Milliseconds);
        }

        private async Task SynchronizeStudentCourses(Student student)
        {
            var studentNumber = student.StudentNumber;
            var studentPassword = await _encryptionService.DecryptAsync(student.StudentPassword);

            var response = await _restApiService.AuthenticateAsync(studentNumber, studentPassword);
            student.AuthKey = response.AuthKey;
            var takenLessons = await _restApiService.StudentTakenLessonsAsync();
            var studentCourses = await GetStudentCoursesAsync(takenLessons, student.Id);
            student.StudentCourses = studentCourses;
            await _context.SaveChangesAsync();
        }

        private async Task<List<StudentCourse>> GetStudentCoursesAsync(IEnumerable<StudentTakenLessons.Response> models,
            int studentId)
        {
            var semesterNotes = await _restApiService.StudentSemesterNotesAsync();
            var studentCourses = new List<StudentCourse>();
            foreach (var model in models)
            {
                var lesson = semesterNotes.GetLessonByCodeAndName(model.Code, model.Name);
                var assessments = await GetAssessmentsAsync(lesson, studentId);
                var studentCourse = await GetStudentCourseAsync(model, assessments, studentId);
                studentCourse.AverageAssessment = lesson?.Exams.FirstOrDefault(x => string.IsNullOrEmpty(x.Avg))?.Avg;
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
            
            return studentCourse is {IsActive: true}
                ? studentCourse
                : new StudentCourse
                {
                    TheoryAbsent = model.TheoryAbsent.AsInt(),
                    PracticeAbsent = model.PracticeAbsent.AsInt(),
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
            List<Assessment> assessmentsToCreate;
            if (studentCourse == null || studentCourse.Assessments.Count == 0 || !studentCourse.IsActive)
            {
                assessmentsToCreate = model.Exams.Select(x => new Assessment
                {
                    Type = x.Name,
                    Point = !string.IsNullOrEmpty(x.Mark) ? x.Mark.AsInt() : 0,
                }).ToList();
            }
            else
            {
                var examNames = model.Exams.ToDictionary(x => x.Name);
                var assessmentsToDelete = studentCourse.Assessments
                    .Where(x => !examNames.ContainsKey(x.Type)).ToArray();
                var assessmentsToUpdate = studentCourse.Assessments
                    .Where(x => examNames.ContainsKey(x.Type))
                    .Select(x =>
                    {
                        x.Point = examNames[x.Type].Mark.AsInt();
                        return x;
                    })
                    .ToDictionary(x => x.Type);

                if (assessmentsToDelete.Any())
                {
                    _context.Assessments.RemoveRange(assessmentsToDelete);
                    await _context.SaveChangesAsync();
                }

                if (assessmentsToUpdate.Any())
                {
                    _context.Assessments.UpdateRange(assessmentsToUpdate.Values);
                    await _context.SaveChangesAsync();
                }

                assessmentsToCreate = examNames.Values
                    .Where(x => !assessmentsToUpdate.ContainsKey(x.Name))
                    .Select(x => new Assessment
                    {
                        Type = x.Name,
                        Point = !string.IsNullOrEmpty(x.Mark) ? x.Mark.AsInt() : 0,
                    }).ToList();
            }

            return assessmentsToCreate;
        }

        private async Task<Course> GetCourseAsync(StudentTakenLessons.Response model)
        {
            return await _context.Courses
                       .FirstOrDefaultAsync(x => x.Code == model.Code && x.Name == model.Name) ??
                   new Course
                   {
                       Code = model.Code,
                       Name = model.Name,
                       Credits = model.Credit.AsInt(),
                       Practice = model.Practice.AsInt(),
                       Theory = model.Theory.AsInt(),
                   };
        }
    }
}