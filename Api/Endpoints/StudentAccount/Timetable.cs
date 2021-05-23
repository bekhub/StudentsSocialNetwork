using System.Collections.Generic;
using System.Net;
using Api.Configuration;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Services;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.StudentAccount
{
    public class Timetable : BaseEndpoint
        .WithoutRequest
        .WithResponse<List<Response.Timetable>>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly StudentAccountService _studentAccountService;

        public Timetable(ICurrentUserAccessor currentUserAccessor, StudentAccountService studentAccountService)
        {
            _currentUserAccessor = currentUserAccessor;
            _studentAccountService = studentAccountService;
        }

        [JwtAuthorize(RoleConstants.STUDENT)]
        [HttpGet("api/timetable")]
        [SwaggerOperation(
            Summary = "Student's timetable",
            Description = "Student's timetable",
            OperationId = "studentAccount.timetable",
            Tags = new[] { "StudentAccount" })]
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(List<Response.Timetable>))]
        public override ActionResult<List<Response.Timetable>> Handle()
        {
            var userId = _currentUserAccessor.GetCurrentUserId();
            return _studentAccountService.MakeRandomTimetable(userId);
        }
    }
}
