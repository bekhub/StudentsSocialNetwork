using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.StudentAccount
{
    public class UpdatePassword : BaseAsyncEndpoint
        .WithRequest<Request.UpdatePassword>
        .WithoutResponse
    {
        private readonly StudentAccountService _service;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IEncryptionService _encryptionService;
        private readonly SsnDbContext _context;

        public UpdatePassword(ICurrentUserAccessor currentUserAccessor, StudentAccountService service,
            IEncryptionService encryptionService, SsnDbContext context)
        {
            _currentUserAccessor = currentUserAccessor;
            _service = service;
            _encryptionService = encryptionService;
            _context = context;
        }

        [JwtAuthorize(RoleConstants.STUDENT)]
        [HttpPost("api/change-student-password")]
        [SwaggerOperation(
            Summary = "Change student's password",
            Description = "Change student's password, that is used to access 'obis.manas.edu.kg'",
            OperationId = "studentAccount.changePassword",
            Tags = new[] { "StudentAccount" })]
        public override async Task<ActionResult> HandleAsync(Request.UpdatePassword request, CancellationToken cancellationToken = new())
        {
            var userId = _currentUserAccessor.GetCurrentUserId();
            var student = await _service.GetCurrentStudentAsync(userId);
            if (!await _service.CheckStudentPasswordAsync(student.StudentNumber, request.Password))
                return BadRequest(Result.InvalidPassword);
            
            var password = await _encryptionService.EncryptAsync(request.Password);
            student.StudentPassword = password;
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(Result.PasswordUpdated);
        }
    }
}
