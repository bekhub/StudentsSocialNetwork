using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Auth
{
    public class Authenticate : BaseAsyncEndpoint
        .WithRequest<Request.Authenticate>
        .WithResponse<Response.Authenticate>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public Authenticate(SignInManager<ApplicationUser> signInManager, IJwtFactory jwtFactory, UserManager<ApplicationUser> userManager, IStudentRepository studentRepository)
        {
            _signInManager = signInManager;
            _jwtFactory = jwtFactory;
            _userManager = userManager;
            _studentRepository = studentRepository;
        }

        [HttpPost("api/authenticate")]
        [SwaggerOperation(
            Summary = "Authenticates a user",
            Description = "Authenticates a user",
            OperationId = "auth.authenticate",
            Tags = new[] { "AuthEndpoints" })]
        public override async Task<ActionResult<Response.Authenticate>> HandleAsync(Request.Authenticate request, 
            CancellationToken cancellationToken = default)
        {
            ApplicationUser user;
            if (string.IsNullOrEmpty(request.Username))
            {
                var student = await _studentRepository.GetByStudentNumberAsync(request.StudentNumber);
                user = student?.User;
            }
            else user = await _userManager.FindByNameAsync(request.Username);

            if (user == null) return Unauthorized();
            
            var result = await _signInManager.PasswordSignInAsync(user?.UserName, request.Password, false, false);

            var response = new Response.Authenticate();

            if (!result.Succeeded) return Unauthorized();
            
            response.Username = user.UserName;
            response.Token = await _jwtFactory.CreateTokenAsync(user.Id, user.Email);

            return response;
        }
    }
}
