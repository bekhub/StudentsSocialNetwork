using System.Diagnostics;
using Api.Configuration;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class MetaController : ControllerBase
    {
        private readonly ICurrentUserAccessor _userAccessor;

        public MetaController(ICurrentUserAccessor userAccessor)
        {
            _userAccessor = userAccessor;
        }

        [HttpGet("/info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var creationDate = System.IO.File.GetCreationTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {creationDate:dd/MM/yyyy HH:mm:ss}");
        }
        
        [HttpGet("/ip-address")]
        public ActionResult<object> IpAddress()
        {
            return Ok(new
            {
                IpAddress = _userAccessor.GetIpAddress(),
            });
        }

        [JwtAuthorize]
        [HttpGet("/protected-route")]
        public ActionResult<object> ProtectedRoute()
        {
            var user = _userAccessor.GetCurrentUser();
            return Ok(new {username = user.UserName});
        }
    }
}
