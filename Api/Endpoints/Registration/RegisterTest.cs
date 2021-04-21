﻿using System;
using System.Diagnostics;
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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Registration
{
    public class RegisterTest : BaseAsyncEndpoint
        .WithRequest<Request.Register>
        .WithoutResponse
    {
        private readonly EmailService _emailService;
        private readonly RegistrationService _registrationService;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<RegisterTest> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        private const string FOLDER = "profiles_pictures";

        public RegisterTest(EmailService emailService, IMapper mapper, UserManager<ApplicationUser> userManager, 
            ILogger<RegisterTest> logger, IFileSystem fileSystem, RegistrationService registrationService)
        {
            _emailService = emailService;
            _mapper = mapper;
            _userManager = userManager;
            _logger = logger;
            _fileSystem = fileSystem;
            _registrationService = registrationService;
        }

        [HttpPost("api/register-test")]
        [SwaggerOperation(
            Summary = "Test registration",
            Description = "Test registration without saving user",
            OperationId = "auth.testSignUp",
            Tags = new[] { "Auth.SignUp" })]
        public override async Task<ActionResult> HandleAsync([FromForm] Request.Register request, 
            CancellationToken cancellationToken = new())
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogWarning("Registration start");
            if (await CheckIsUserRegistered(request))
                return BadRequest(Result.UserExists);

            var user = _mapper.Map<ApplicationUser>(request);
            user.SignUpDate = DateTime.UtcNow;
            user.ProfilePictureUrl = await MakePictureUrlAsync(request.ProfilePicture);
            
            var student = await _registrationService.GetUnregisteredStudentAsync(request.StudentNumber, request.StudentPassword);
            if (student == null) return BadRequest(Result.RegisterError);

            user.Firstname = student.Firstname;
            user.Lastname = student.Lastname;

            user.Students.Add(student);
            
            _logger.LogWarning("Student {StudentNumber} create account with @{Username} username", 
                request.StudentNumber, request.Username);

            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, Request.Host.Value);
            stopWatch.Stop();
            _logger.LogWarning("Registration end. {Milliseconds} ms elapsed", stopWatch.Elapsed.Milliseconds);

            return Ok(Result.RegisterSuccess);
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

        private async Task<string> MakePictureUrlAsync(IFormFile file)
        {
            if (file is not {Length: > 0}) return string.Empty;

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