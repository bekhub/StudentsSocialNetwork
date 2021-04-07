using System;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Services;
using Core.ObisApiModels;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class StudentsService
    {
        private readonly SsnDbContext _context;
        private readonly IRestApiService _restApiService;
        private readonly IEncryptionService _encryptionService;

        public StudentsService(SsnDbContext ssnDbContext, IRestApiService restApiService, 
            IEncryptionService encryptionService)
        {
            _context = ssnDbContext;
            _restApiService = restApiService;
            _encryptionService = encryptionService;
        }

        public async Task<Student> GetStudentAsync(string studentNumber)
        {
            return await _context.Students.SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
        }
        
        public async Task<Student> GetUnregisteredStudentAsync(string studentNumber, string studentPassword)
        {
            var student = await _context.Students.SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
            if (student?.UserId != null) return null;
            return student ?? await BuildStudentAsync(studentNumber, studentPassword);
        }

        public async Task<Student> BuildStudentAsync(string studentNumber, string studentPassword)
        {
            var authenticate = await _restApiService.AuthenticateAsync(new Authenticate.Request
            {
                Number = studentNumber,
                Password = studentPassword,
            });
            if (authenticate == null || string.IsNullOrEmpty(authenticate.AuthKey))
                return null;

            var mainInfo = await _restApiService.MainInfoAsync();
            var studentInfo = await _restApiService.StudentInfoAsync();

            var encryptedPassword = await _encryptionService.EncryptAsync(studentPassword);
            var admissionYear = Convert.ToInt32($"20{studentNumber[..2]}");
            var department = await GetDepartmentAsync(mainInfo.Department, mainInfo.Faculty);
            var studentEmail = GetStudentEmail(studentNumber);
            return new Student
            {
                StudentNumber = studentNumber,
                StudentPassword = encryptedPassword,
                StudentEmail = studentEmail,
                Firstname = studentInfo.Name,
                Lastname = studentInfo.Surname,
                AuthKey = authenticate.AuthKey,
                AdmissionYear = admissionYear,
                BirthPlace = studentInfo.Birthplace,
                BirthDate = DateTime.Parse(studentInfo.Birthday),
                Department = department,
            };
        }

        private async Task<Institute> GetInstituteAsync(string name)
        {
            return await _context.Institutes.SingleOrDefaultAsync(x => x.Name == name) ?? new Institute
            {
                Name = name,
            };
        }

        private async Task<Department> GetDepartmentAsync(string departmentName, string instituteName)
        {
            return await _context.Departments.SingleOrDefaultAsync(x => x.Name == departmentName) ?? new Department
            {
                Name = departmentName,
                Institute = await GetInstituteAsync(instituteName),
            };
        }

        private string GetStudentEmail(string studentNumber)
        {
            return $"{studentNumber}@manas.edu.kg";
        }
    }
}
