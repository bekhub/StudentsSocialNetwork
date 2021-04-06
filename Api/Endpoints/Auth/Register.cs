using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Api.Services;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using FluentValidation;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class Register : BaseAsyncEndpoint
        .WithRequest<Request.Register>
        .WithoutResponse
    {
        private readonly EmailService _emailService;
        private readonly IRestApiService _restApiService;
        private readonly IEncryptionService _encryptionService;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<Register> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;

        private const string FOLDER = "profiles_pictures";

        public Register(EmailService emailService, IMapper mapper, UserManager<ApplicationUser> userManager, 
            ILogger<Register> logger, IRestApiService restApiService, IEncryptionService encryptionService, 
            SsnDbContext context, IFileSystem fileSystem)
        {
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _restApiService = restApiService;
            _encryptionService = encryptionService;
            _context = context;
            _fileSystem = fileSystem;
        }

        [HttpPost("api/register")]
        [SwaggerOperation(
            Summary = "Registers a user",
            Description = "Registers a user",
            OperationId = "auth.signUp",
            Tags = new[] { "AuthEndpoints" })]
        public override async Task<ActionResult> HandleAsync([FromForm] Request.Register request, 
            CancellationToken cancellationToken = new())
        {
            var origin = Request.Headers["origin"];
            if (await _userManager.FindByEmailAsync(request.Email) != null)
            {
                await _emailService.SendAlreadyRegisteredEmailAsync(request.Email, origin);
                return Ok("User already registered");
            }

            var user = _mapper.Map<ApplicationUser>(request);
            user.SignUpDate = DateTime.UtcNow;
            user.ProfilePictureUrl = await MakePictureUrlAsync(request.ProfilePicture);
            
            var student = await BuildStudentAsync(request.StudentNumber, request.StudentPassword);
            if (student == null) return BadRequest("Error during registering user");
            
            user.Students.Add(student);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return BadRequest("Error during registering user");
            
            await _userManager.AddToRolesAsync(user, new[] {RoleConstants.USER, RoleConstants.STUDENT});
            
            _logger.LogInformation("User created a new account with password.");

            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, origin);

            return Ok("User created");
        }

        private async Task<string> MakePictureUrlAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0) return string.Empty;

            await using var stream = new MemoryStream();
            await file.CopyToAsync(stream);
            
            var picName = _fileSystem.GeneratePictureName(file.FileName);
            var picture = stream.ToArray();

            if (!await _fileSystem.SavePictureAsync(picName, picture, FOLDER))
                return string.Empty;
            
            return _fileSystem.MakePictureUrl(picName, FOLDER);
        }

        private async Task<Student> BuildStudentAsync(string studentNumber, string studentPassword)
        {
            var authenticate = await _restApiService.AuthenticateAsync(new Core.ObisApiModels.Authenticate.Request
            {
                Number = studentNumber,
                Password = studentPassword,
            });
            if (authenticate == null || string.IsNullOrEmpty(authenticate.AuthKey))
                return null;
            _restApiService.SetAuthKey(authenticate.AuthKey);

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
        
        public class RequestValidator : AbstractValidator<Request.Register>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.StudentNumber).NotNull().NotEmpty();
                RuleFor(x => x.StudentPassword).NotNull().NotEmpty();
            }
        }
    }
}
