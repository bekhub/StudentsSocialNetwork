using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Services;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.StudentAccount
{
    public class StudentAccountService
    {
        private readonly SsnDbContext _context;
        private readonly StudentsService _studentsService;
        private readonly IFireAndForgetHandler _fireAndForgetHandler;

        public StudentAccountService(SsnDbContext context, IFireAndForgetHandler fireAndForgetHandler,
            StudentsService studentsService)
        {
            _context = context;
            _fireAndForgetHandler = fireAndForgetHandler;
            _studentsService = studentsService;
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
    }
}
