using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.UserAccount
{
    public class ChangePassword : BaseAsyncEndpoint
        .WithRequest<Request.ChangePassword>
        .WithResponse<Result>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public ChangePassword(UserManager<ApplicationUser> userManager, ICurrentUserAccessor currentUserAccessor)
        {
            _userManager = userManager;
            _currentUserAccessor = currentUserAccessor;
        }

        [JwtAuthorize(RoleConstants.USER)]
        [HttpPut("api/change-password")]
        [SwaggerOperation(
            Summary = "Change user's password",
            Description = "Change user's password",
            OperationId = "userAccount.changePassword",
            Tags = new[] { "UserAccount" })]
        public override async Task<ActionResult<Result>> HandleAsync(Request.ChangePassword request, CancellationToken cancellationToken = new())
        {
            var user = await _currentUserAccessor.GetCurrentUserAsync(cancellationToken);
            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            if (!result.Succeeded)
                return BadRequest(Result.From(Resource.PasswordError));
            return Ok(Result.From(Resource.PasswordChanged));
        }
    }
}
