using System;
using System.IO;
using System.Threading.Tasks;
using Api.Helpers.Extensions;
using Api.Services;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.ObisApiModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Api.Endpoints.Registration
{
    public class RegistrationService
    {
        private readonly SsnDbContext _context;
        private readonly IFireAndForgetHandler _fireAndForgetHandler;
        private readonly IObisApiService _obisApiService;
        private readonly IEncryptionService _encryptionService;
        private readonly IFileSystem _fileSystem;

        public RegistrationService(SsnDbContext context, IObisApiService obisApiService,
            IEncryptionService encryptionService, IFireAndForgetHandler fireAndForgetHandler, IFileSystem fileSystem)
        {
            _context = context;
            _obisApiService = obisApiService;
            _encryptionService = encryptionService;
            _fireAndForgetHandler = fireAndForgetHandler;
            _fileSystem = fileSystem;
        }

        public async Task<bool> CheckStudentAsync(string studentNumber, string studentPassword)
        {
            var response = await _obisApiService.AuthenticateAsync(new Authenticate.Request
            {
                Number = studentNumber,
                Password = studentPassword,
            });
            return response != null && !string.IsNullOrEmpty(response.AuthKey);
        }

        public Task<Student> GetStudentAsync(string studentNumber)
        {
            return _context.Students.SingleOrDefaultAsync(x => x.StudentNumber == studentNumber);
        }

        public async Task<Student> GetUnregisteredStudentAsync(string studentNumber, string studentPassword)
        {
            var student = await GetStudentAsync(studentNumber);
            if (student?.UserId != null) return null;
            return student ?? await BuildStudentAsync(studentNumber, studentPassword);
        }

        public void SynchronizeStudentCourses(string studentNumber)
        {
            _fireAndForgetHandler.Execute<StudentsService>(async studentsService =>
                    await studentsService.SynchronizeStudentCoursesAsync(studentNumber));
        }

        private async Task<Student> BuildStudentAsync(string studentNumber, string studentPassword)
        {
            var authenticate = await _obisApiService.AuthenticateAsync(studentNumber, studentPassword);
            if (authenticate == null || string.IsNullOrEmpty(authenticate.AuthKey))
                return null;

            var mainInfo = await _obisApiService.MainInfoAsync();
            var studentInfo = await _obisApiService.StudentInfoAsync();

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
        
        public async Task<string> MakePictureUrlAsync(IFormFile file, string folder)
        {
            if (file is not {Length: > 0}) return string.Empty;

            var picName = GeneratePictureName(file.FileName);
            var picture = file.ToArray();

            var pictureUrl = await _fileSystem.SavePictureAsync(picName, picture, folder);

            return pictureUrl ?? string.Empty;
        }

        private static string GetStudentEmail(string studentNumber)
        {
            return $"{studentNumber}@manas.edu.kg";
        }
        
        private static string GeneratePictureName(string pictureName)
        {
            return $"{Guid.NewGuid()}{Path.GetExtension(pictureName)}";
        }
    }
}
