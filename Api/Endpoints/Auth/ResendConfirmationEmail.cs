using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Services;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class ResendConfirmationEmail : BaseAsyncEndpoint
        .WithRequest<string>
        .WithoutResponse
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
            Summary = "Resend confirmation email",
            Description = "Resend confirmation email",
            OperationId = "auth.resendConfirmationEmail",
            Tags = new[] { "Auth.SignUp" })]
        public override async Task<ActionResult> HandleAsync(string email, 
            CancellationToken cancellationToken = new())
        {
            if (string.IsNullOrEmpty(email)) return BadRequest(Result.EmailRequired);
            var origin = Request.Host.Value;

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return BadRequest(Result.UserNotFound);
            
            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _emailService.SendVerificationEmailAsync(user.Email, verificationToken, origin);

            return Ok(Result.EmailSent);
        }
    }
}
