using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.StudentAccount
{
    public class StudentAccountService
    {
        private readonly SsnDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentAccountService(SsnDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
                .Where(x => x.Student.UserId == userId)
                .ToListAsync();
        }
    }
}
