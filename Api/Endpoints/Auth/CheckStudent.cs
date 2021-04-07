using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using AuthenticateRequest = Core.ObisApiModels.Authenticate.Request;

namespace Api.Endpoints.Auth
{
    public class CheckStudent : BaseAsyncEndpoint
        .WithRequest<Request.CheckStudent>
        .WithResponse<Response.CheckStudent>
    {
        private readonly IRestApiService _restApiService;

        public CheckStudent(IRestApiService restApiService)
        {
            _restApiService = restApiService;
        }

        [HttpPost("api/check-student")]
        [SwaggerOperation(
            Summary = "Checks whether the student exists",
            Description = "Checks whether the student exists",
            OperationId = "auth.checkStudent",
            Tags = new[] { "AuthEndpoints" })]
        public override async Task<ActionResult<Response.CheckStudent>> HandleAsync(Request.CheckStudent request, 
            CancellationToken cancellationToken = new())
        {
            var response = await _restApiService.AuthenticateAsync(new AuthenticateRequest
            {
                Number = request.StudentNumber,
                Password = request.StudentPassword,
            });
            var isExist = response != null && !string.IsNullOrEmpty(response.AuthKey);
            var message = isExist
                ? "The student exists"
                : "The student does not exist";
            
            return new Response.CheckStudent
            {
                IsExist = isExist,
                Message = message,
            };
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
