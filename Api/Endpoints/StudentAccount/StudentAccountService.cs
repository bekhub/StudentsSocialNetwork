﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Services;
using Core.Entities;
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
        private readonly IEncryptionService _encryptionService;
        private readonly IObisApiService _obisApiService;

        public StudentAccountService(SsnDbContext context,
            StudentsService studentsService, IEncryptionService encryptionService, IObisApiService obisApiService)
        {
            _context = context;
            _studentsService = studentsService;
            _encryptionService = encryptionService;
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
    }
}
