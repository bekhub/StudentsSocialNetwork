using System;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Helpers.Extensions;
using Api.Services;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class Register : BaseAsyncEndpoint
        .WithRequest<Request.Register>
        .WithoutResponse
    {
        private readonly EmailService _emailService;
        private readonly StudentsService _studentsService;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<Register> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        private const string FOLDER = "profiles_pictures";

        public Register(EmailService emailService, IMapper mapper, UserManager<ApplicationUser> userManager, 
            ILogger<Register> logger, IFileSystem fileSystem, StudentsService studentsService)
        {
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _fileSystem = fileSystem;
            _studentsService = studentsService;
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
            var student = await _studentsService.GetStudentAsync(request.StudentNumber);
            if (await _userManager.FindByEmailAsync(request.Email) != null ||
                await _userManager.FindByNameAsync(request.Username) != null ||
                student?.UserId != null)
            {
                await _emailService.SendAlreadyRegisteredEmailAsync(request.Email, origin);
                return BadRequest(Result.UserExists);
            }

            var user = _mapper.Map<ApplicationUser>(request);
            user.SignUpDate = DateTime.UtcNow;
            user.ProfilePictureUrl = await MakePictureUrlAsync(request.ProfilePicture);
            
            student = await _studentsService.GetUnregisteredStudentAsync(request.StudentNumber, request.StudentPassword);
            if (student == null) return BadRequest(Result.RegisterError);
            
            user.Students.Add(student);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return BadRequest(Result.RegisterError);
            
            await _userManager.AddToRolesAsync(user, new[] {RoleConstants.USER, RoleConstants.STUDENT});
            
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            _logger.LogInformation($"Student {request.StudentNumber} create account with @{request.Username} username");

            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, origin);

            return Ok("User created");
        }

        private async Task<string> MakePictureUrlAsync(IFormFile file)
        {
            if (file == null || file.Length <= 0) return string.Empty;

            var picName = _fileSystem.GeneratePictureName(file.FileName);
            var picture = file.ToArray();

            if (!await _fileSystem.SavePictureAsync(picName, picture, FOLDER))
                return string.Empty;
            
            return _fileSystem.MakePictureUrl(picName, FOLDER);
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
