using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class Authenticate : BaseAsyncEndpoint
        .WithRequest<Request.Authenticate>
        .WithResponse<Response.Authenticate>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtConfiguration _jwtConfiguration;
        private readonly ICurrentUserAccessor _userAccessor;

        public Authenticate(SignInManager<ApplicationUser> signInManager, IJwtFactory jwtFactory, 
            IStudentRepository studentRepository, IOptions<JwtConfiguration> jwtConfiguration, 
            IUserRepository userRepository, ICurrentUserAccessor userAccessor)
        {
            _signInManager = signInManager;
            _jwtFactory = jwtFactory;
            _studentRepository = studentRepository;
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        [HttpPost("api/authenticate")]
        [SwaggerOperation(
            Summary = "Authenticates the user",
            Description = "Authenticates the user",
            OperationId = "auth.authenticate",
            Tags = new[] { "Auth.SignIn" })]
        public override async Task<ActionResult<Response.Authenticate>> HandleAsync(Request.Authenticate request, 
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrEmpty(request.Username) && !string.IsNullOrEmpty(request.StudentNumber) || 
                string.IsNullOrEmpty(request.Username) && string.IsNullOrEmpty(request.StudentNumber))
                return BadRequest("Please provide one of two: either the username or student number");
            
            ApplicationUser user;
            if (string.IsNullOrEmpty(request.Username))
            {
                var student = await _studentRepository.GetByStudentNumberAsync(request.StudentNumber);
                user = student?.User;
            }
            else user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null) return Unauthorized();
            
            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!result.Succeeded) return Unauthorized();

            var jwtToken = await _jwtFactory.CreateTokenAsync(user.Id, user.Email);
            var refreshToken = _jwtFactory.CreateRefreshToken(_userAccessor.GetIpAddress());
            user.RefreshTokens.Add(refreshToken);
            
            user.RemoveOldRefreshTokens(_jwtConfiguration.RefreshTokenTtl);

            await _userRepository.UpdateAsync(user);

            return new Response.Authenticate
            {
                Username = user.UserName,
                JwtToken = jwtToken,
                RefreshToken = refreshToken.Token,
            };
        }
    }
}
