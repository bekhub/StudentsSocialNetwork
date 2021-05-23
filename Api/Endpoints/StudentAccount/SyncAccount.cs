using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.StudentAccount
{
    public class SyncAccount : BaseAsyncEndpoint
        .WithoutRequest
        .WithoutResponse
    {
        private readonly StudentAccountService _service;
        private readonly ICurrentUserAccessor _userAccessor;

        public SyncAccount(ICurrentUserAccessor userAccessor, StudentAccountService studentAccountService)
        {
            _userAccessor = userAccessor;
            _service = studentAccountService;
        }


        [JwtAuthorize(RoleConstants.STUDENT)]
        [HttpPost("api/sync-account")]
        [SwaggerOperation(
            Summary = "Synchronize student's account",
            Description = "Synchronize student's account",
            OperationId = "studentAccount.syncAccount",
            Tags = new[] { "StudentAccount" })]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public override async Task<ActionResult> HandleAsync(CancellationToken cancellationToken = new())
        {
            var userId = _userAccessor.GetCurrentUserId();
            
            await _service.SynchronizeStudentCoursesAsync(userId);
            
            return Ok();
        }
    }
}
