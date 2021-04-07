﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class VerifyEmail : BaseAsyncEndpoint
        .WithRequest<Request.VerifyEmail>
        .WithResponse<Response.VerifyEmail>

    {
        private readonly UserManager<ApplicationUser> _userManager;

        public VerifyEmail(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("api/verify-email")]
        [SwaggerOperation(
            Summary = "Verifies email",
            Description = "Verifies email",
            OperationId = "auth.verifyEmail",
            Tags = new[] { "AuthEndpoints" })]
        public override async Task<ActionResult<Response.VerifyEmail>> HandleAsync([FromQuery] Request.VerifyEmail request,
            CancellationToken cancellationToken = new())
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
                return BadRequest(new Response.VerifyEmail(false));

            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (!result.Succeeded)
                return BadRequest(new Response.VerifyEmail(false));

            return Ok(new Response.VerifyEmail(true));
        }
        
        public class RequestValidator : AbstractValidator<Request.VerifyEmail>
        {
            public RequestValidator()
            {
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Token).NotNull().NotEmpty();
            }
        }
    }
}
