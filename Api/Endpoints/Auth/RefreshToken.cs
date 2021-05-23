using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class RefreshToken : BaseAsyncEndpoint
        .WithRequest<string>
        .WithResponse<Response.Authenticate>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ICurrentUserAccessor _userAccessor;

        public RefreshToken(IJwtFactory jwtFactory, IUserRepository userRepository, 
            IOptions<JwtConfiguration> jwtConfiguration, ICurrentUserAccessor userAccessor)
        {
            _jwtFactory = jwtFactory;
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        [HttpPost("api/refresh-token")]
        [SwaggerOperation(
            Summary = "Refreshes access token",
            Description = "Provide refresh token",
            OperationId = "auth.refreshToken",
            Tags = new[] { "Auth.SignIn" })]
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(Response.Authenticate))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, null, typeof(Result))]
        public override async Task<ActionResult<Response.Authenticate>> HandleAsync(string token, 
            CancellationToken cancellationToken = new())
        {
            if (string.IsNullOrEmpty(token)) return Result.BadRequest(Resource.TokenRequired);
            var (user, refreshToken) = await _userRepository.GetByRefreshTokenAsync(token, cancellationToken);
            if (user == null || !refreshToken.IsActive) return Result.BadRequest(Resource.TokenInvalid);

            var ipAddress = _userAccessor.GetIpAddress();
            var newRefreshToken = _jwtFactory.CreateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
            user.RefreshTokens.Add(newRefreshToken);
            user.RemoveOldRefreshTokens(_jwtConfiguration.RefreshTokenTtl);

            await _userRepository.UpdateAsync(user);
            var (jwtToken, jwtExpires) = await _jwtFactory.CreateTokenAsync(user.Id, user.Email);

            return new Response.Authenticate
            {
                Username = user.UserName,
                UserId = user.Id,
                JwtToken = jwtToken,
                JwtExpires = jwtExpires,
                RefreshToken = newRefreshToken.Token,
                RefreshExpires = newRefreshToken.Expires,
            };
        }
    }
}
