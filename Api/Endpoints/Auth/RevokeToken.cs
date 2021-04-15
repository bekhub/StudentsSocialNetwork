using System;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class RevokeToken : BaseAsyncEndpoint
        .WithRequest<string>
        .WithoutResponse
    {
        private readonly ICurrentUserAccessor _userAccessor;
        private readonly IUserRepository _userRepository;

        public RevokeToken(ICurrentUserAccessor userAccessor, IUserRepository userRepository)
        {
            _userAccessor = userAccessor;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("api/revoke-token")]
        [SwaggerOperation(
            Summary = "Revokes refresh token",
            Description = "Provide refresh token",
            OperationId = "auth.revokeToken",
            Tags = new[] { "Auth.SignIn" })]
        public override async Task<ActionResult> HandleAsync(string token, CancellationToken cancellationToken = new())
        {
            if (string.IsNullOrEmpty(token)) return BadRequest("Token is required");
            var user = await _userAccessor.GetCurrentUserAsync(cancellationToken);
            if (!user.OwnsToken(token) && !User.IsInRole(RoleConstants.ADMINISTRATOR))
                return Unauthorized("Invalid token");

            var refreshToken = user.GetRefreshToken(token);
            
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = _userAccessor.GetIpAddress();
            await _userRepository.UpdateAsync(user);

            return Ok("Token revoked");
        }
    }
}
