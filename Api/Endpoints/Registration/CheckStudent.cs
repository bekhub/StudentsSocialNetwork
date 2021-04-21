using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Registration
{
    public class CheckStudent : BaseAsyncEndpoint
        .WithRequest<Request.CheckStudent>
        .WithResponse<Response.CheckStudent>
    {
        private readonly RegistrationService _registrationService;

        public CheckStudent(RegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        [HttpPost("api/check-student")]
        [SwaggerOperation(
            Summary = "Checks whether the student exists",
            Description = "Checks whether the student exists",
            OperationId = "auth.checkStudent",
            Tags = new[] { "Auth.SignUp" })]
        public override async Task<ActionResult<Response.CheckStudent>> HandleAsync(Request.CheckStudent request, 
            CancellationToken cancellationToken = new())
        {
            var isExist = await _registrationService.CheckStudentAsync(request.StudentNumber, request.StudentPassword);

            return isExist
                ? Ok(new Response.CheckStudent {IsExist = true, Message = "The student exists"})
                : BadRequest(new Response.CheckStudent {IsExist = false, Message = "The student does not exist"});
        }
        
        public class RequestValidator : AbstractValidator<Request.CheckStudent>
        {
            public RequestValidator()
            {
                RuleFor(x => x.StudentNumber).NotNull().NotEmpty();
                RuleFor(x => x.StudentPassword).NotNull().NotEmpty();
            }
        }
    }
}
