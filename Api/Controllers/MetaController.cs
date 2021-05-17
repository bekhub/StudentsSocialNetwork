using System.Diagnostics;
using System.Threading.Tasks;
using Api.Configuration;
using Core.Interfaces.Services;
using Core.ObisApiModels;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("meta")]
    public class MetaController : ControllerBase
    {
        private readonly ICurrentUserAccessor _userAccessor;
        private readonly IObisApiService _obisApiService;

        public MetaController(ICurrentUserAccessor userAccessor, IObisApiService obisApiService)
        {
            _userAccessor = userAccessor;
            _obisApiService = obisApiService;
        }

        [HttpGet("info")]
        public ActionResult<string> Info()
        {
            var assembly = typeof(Startup).Assembly;

            var creationDate = System.IO.File.GetCreationTime(assembly.Location);
            var version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;

            return Ok($"Version: {version}, Last Updated: {creationDate:dd/MM/yyyy HH:mm:ss}");
        }
        
        [HttpGet("ip-address")]
        public ActionResult<object> IpAddress()
        {
            return Ok(new
            {
                IpAddress = _userAccessor.GetIpAddress(),
            });
        }

        [JwtAuthorize]
        [HttpGet("protected-route")]
        public ActionResult<object> ProtectedRoute()
        {
            var user = _userAccessor.GetCurrentUser();
            return Ok(new {username = user.UserName});
        }

        [HttpGet("wake-up")]
        public Task<WakeUp.Response> WakeUp()
        {
            return _obisApiService.WakeUpAsync();
        }
    }
}
