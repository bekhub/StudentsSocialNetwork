using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
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

        [JwtAuthorize]
        [HttpPost("api/revoke-token")]
        [SwaggerOperation(
            Summary = "Revokes refresh token",
            Description = "Provide refresh token. Requires authorization",
            OperationId = "auth.revokeToken",
            Tags = new[] { "Auth.SignIn" })]
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(Result))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(Result))]
        public override async Task<ActionResult> HandleAsync(string token, CancellationToken cancellationToken = new())
        {
            if (string.IsNullOrEmpty(token)) return Result.BadRequest(Resource.TokenRequired);
            var user = await _userAccessor.GetCurrentUserAsync(cancellationToken);
            var refreshToken = user.GetRefreshToken(token);
            
            if (refreshToken == null) return Result.BadRequest(Resource.TokenInvalid);

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = _userAccessor.GetIpAddress();
            await _userRepository.UpdateAsync(user);

            return Result.Ok(Resource.TokenRevoked);
        }
    }
}
