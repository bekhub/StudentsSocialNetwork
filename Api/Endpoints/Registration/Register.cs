using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Helpers.Extensions;
using Api.Resources;
using Api.Services;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using FluentValidation;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Registration
{
    public class Register : BaseAsyncEndpoint
        .WithRequest<Request.Register>
        .WithResponse<Result>
    {
        private readonly EmailService _emailService;
        private readonly RegistrationService _registrationService;
        private readonly ILogger<Register> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;

        private const string FOLDER = "profiles_pictures";

        public Register(EmailService emailService, IMapper mapper, UserManager<ApplicationUser> userManager, 
            ILogger<Register> logger, RegistrationService registrationService, IFileSystem fileSystem)
        {
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _registrationService = registrationService;
            _fileSystem = fileSystem;
        }

        [HttpPost("api/register")]
        [SwaggerOperation(
            Summary = "Registers a user",
            Description = "Registers a user",
            OperationId = "auth.signUp",
            Tags = new[] { "Auth.SignUp" })]
        public override async Task<ActionResult<Result>> HandleAsync([FromForm] Request.Register request, 
            CancellationToken cancellationToken = new())
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogWarning("Registration start");
            if (await CheckIsUserRegistered(request))
                return BadRequest(Result.From(DefaultResource.UserExists));

            var user = _mapper.Map<ApplicationUser>(request);
            user.SignUpDate = DateTime.UtcNow;
            var file = request.ProfilePicture;
            user.ProfilePictureUrl = await _fileSystem.SavePictureAsync(file.FileName, file.ToArray(), FOLDER);
            
            var student = await _registrationService.GetUnregisteredStudentAsync(request.StudentNumber, request.StudentPassword);
            if (student == null) return BadRequest(Result.From(DefaultResource.RegisterError));

            user.Firstname = student.Firstname;
            user.Lastname = student.Lastname;

            user.Students.Add(student);
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded) return BadRequest(Result.From(DefaultResource.RegisterError));
            
            await _userManager.AddToRolesAsync(user, new[] {RoleConstants.USER, RoleConstants.STUDENT});
            
            _logger.LogWarning("Student {StudentNumber} create account with @{Username} username", 
                request.StudentNumber, request.Username);
            
            _registrationService.SynchronizeStudentCourses(request.StudentNumber);

            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, Request.Host.Value);
            
            stopWatch.Stop();
            _logger.LogWarning("Registration end. {Milliseconds} ms elapsed", stopWatch.Elapsed.Milliseconds);

            return Ok(Result.From(DefaultResource.RegisterSuccess));
        }

        private async Task<bool> CheckIsUserRegistered(Request.Register request)
        {
            var student = await _registrationService.GetStudentAsync(request.StudentNumber);
            if (await _userManager.FindByEmailAsync(request.Email) == null &&
                await _userManager.FindByNameAsync(request.Username) == null &&
                student?.UserId == null) return false;
            
            await _emailService.SendAlreadyRegisteredEmailAsync(request.Email, Request.Host.Value);
            return true;
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
