using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.StudentAccount
{
    public class PersonalInformation : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response.PersonalInformation>
    {
        private readonly StudentAccountService _studentAccountService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public PersonalInformation(StudentAccountService studentAccountService, IMapper mapper, 
            ICurrentUserAccessor currentUserAccessor)
        {
            _studentAccountService = studentAccountService;
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
        }

        [JwtAuthorize(RoleConstants.STUDENT)]
        [HttpGet("api/personal-information")]
        [SwaggerOperation(
            Summary = "Student's personal information",
            Description = "Student's personal information",
            OperationId = "studentAccount.personalInformation",
            Tags = new[] { "StudentAccount" })]
        public override async Task<ActionResult<Response.PersonalInformation>> HandleAsync(CancellationToken cancellationToken = new())
        {
            var userId = _currentUserAccessor.GetCurrentUserId();
            var student = await _studentAccountService.GetStudentByUserIdAsync(userId);

            return _mapper.Map<Student, Response.PersonalInformation>(student);
        }
    }
}
