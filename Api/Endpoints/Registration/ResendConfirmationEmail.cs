using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Resources;
using Api.Services;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Registration
{
    public class ResendConfirmationEmail : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<Result>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailService _emailService;

        public ResendConfirmationEmail(UserManager<ApplicationUser> userManager, EmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [HttpPost("api/resend-confirmation-email")]
        [SwaggerOperation(
            Summary = "Resends confirmation email",
            Description = "Resends confirmation email if user didn't receive email",
            OperationId = "auth.resendConfirmationEmail",
            Tags = new[] { "Auth.SignUp" })]
        public override async Task<ActionResult<Result>> HandleAsync(string email, 
            CancellationToken cancellationToken = new())
        {
            if (string.IsNullOrEmpty(email)) return BadRequest(Result.From(DefaultResource.EmailRequired));
            var host = Request.Host.Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return BadRequest(Result.From(DefaultResource.UserNotFound));
            
            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, host);

            return Ok(Result.From(DefaultResource.EmailSent));
        }
    }
}
