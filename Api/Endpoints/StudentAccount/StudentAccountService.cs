using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Services;
using Common.Helpers;
using Core.Entities;
using Core.Enums;
using Core.Interfaces.Services;
using Core.ObisApiModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.StudentAccount
{
    public class StudentAccountService
    {
        private readonly SsnDbContext _context;
        private readonly StudentsService _studentsService;
        private readonly IObisApiService _obisApiService;

        public StudentAccountService(SsnDbContext context,
            StudentsService studentsService, IObisApiService obisApiService)
        {
            _context = context;
            _studentsService = studentsService;
            _obisApiService = obisApiService;
        }
        
        public async Task SynchronizeStudentCoursesAsync(string userId)
        {
            var student = await GetCurrentStudentAsync(userId);
            await _studentsService.SynchronizeStudentCoursesAsync(student);
        }

        public async Task<Student> GetCurrentStudentAsync(string userId)
        {
            var user = await _context.Users
                .Include(x => x.Students)
                .SingleOrDefaultAsync(x => x.Id == userId);
            return user.Students.FirstOrDefault(x => x.IsActive.HasValue && x.IsActive.Value);
        }

        public Task<Student> GetStudentByUserIdAsync(string userId)
        {
            return _context.Students
                .Include(x => x.User)
                .Include(x => x.Department)
                .ThenInclude(x => x.Institute)
                .SingleOrDefaultAsync(x => x.UserId == userId);
        }

        public Task<List<StudentCourse>> GetStudentCoursesByUserIdAsync(string userId)
        {
            return _context.StudentCourses
                .Include(x => x.Course)
                .Include(x => x.Student)
                .Include(x => x.Assessments)
                .Where(x => x.Student.UserId == userId && 
                            x.AcademicYear == StudentCourse.CurrentAcademicYear &&
                            x.Semester == StudentCourse.CurrentSemester)
                .ToListAsync();
        }

        public async Task<bool> CheckStudentPasswordAsync(string studentNumber, string studentPassword)
        {
            var response = await _obisApiService.AuthenticateAsync(new Authenticate.Request
            {
                Number = studentNumber,
                Password = studentPassword,
            });
            return response != null && !string.IsNullOrEmpty(response.AuthKey);
        }

        private readonly (string timeFrom, string timeTo)[] _times = {
            ("9:00", "9:35"),
            ("9:45", "10:20"),
            ("10:30", "11:05"),
            ("11:15", "11:50"),
            ("12:00", "12:35"),
            ("13:30", "14:05"),
            ("14:15", "14:50"),
            ("15:00", "15:35"),
            ("15:45", "16:20"),
            ("16:30", "17:05"),
            ("17:15", "17:50"),
        };

        private readonly Response.Weekday[] _weekdays = Enum.GetValues<Weekday>()
            .Select(x => new Response.Weekday((int)x, x.ToString())).ToArray();
        
        public List<Response.Timetable> MakeRandomTimetable(string userId)
        {
            var weekdaysCount = _weekdays.Length - 2;
            var timesCount = _times.Length - 1;
            
            var studentCourses = _context.StudentCourses
                .Include(x => x.Course)
                .Include(x => x.Student)
                .Where(x => x.Student.UserId == userId && 
                            x.AcademicYear == StudentCourse.CurrentAcademicYear &&
                            x.Semester == StudentCourse.CurrentSemester &&
                            x.Course.Theory + x.Course.Practice > 0)
                .AsNoTracking().ToList();
            
            var courses = new Stack<StudentCourse>(studentCourses);
            var totalHours = studentCourses.Sum(x => x.Course.Theory + x.Course.Practice);
            var randoms = Numbers.GenerateRandoms(totalHours, 0, weekdaysCount * timesCount);
            randoms.Sort();
            
            var currentHours = 0;
            var course = courses.Peek();
            var timetable = new List<Response.Timetable>();
            foreach (var i in randoms)
            {
                if (course.Course.Total == currentHours)
                {
                    courses.Pop();
                    course = courses.Peek();
                    currentHours = 0;
                }

                var weekday = _weekdays[i / 10];
                var (timeFrom, timeTo) = _times[i % timesCount];
                timetable.Add(new Response.Timetable
                {
                    Code = course.Course.Code,
                    Name = course.Course.Name,
                    Weekday = weekday,
                    TimeFrom = timeFrom,
                    TimeTo = timeTo,
                    Teacher = "MEHMET KENAN DÖNMEZ",
                    Classroom = "MFFB-522",
                    IsMandatory = true,
                });
                currentHours++;
            }

            return timetable;
        }
    }
}
