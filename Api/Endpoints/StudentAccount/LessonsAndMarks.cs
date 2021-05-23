using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class LessonsAndMarks : BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<List<Response.LessonsAndMarks>>
    {
        private readonly StudentAccountService _studentAccountService;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public LessonsAndMarks(StudentAccountService studentAccountService, IMapper mapper,
            ICurrentUserAccessor currentUserAccessor)
        {
            _studentAccountService = studentAccountService;
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
        }

        [JwtAuthorize(RoleConstants.STUDENT)]
        [HttpGet("api/lessons-and-marks")]
        [SwaggerOperation(
            Summary = "Student's lessons and marks",
            Description = "Student's lessons and marks",
            OperationId = "studentAccount.lessonAndMarks",
            Tags = new[] { "StudentAccount" })]
        [SwaggerResponse((int)HttpStatusCode.OK, null, typeof(List<Response.LessonsAndMarks>))]
        public override async Task<ActionResult<List<Response.LessonsAndMarks>>> HandleAsync(CancellationToken cancellationToken = new())
        {
            var userId = _currentUserAccessor.GetCurrentUserId();
            var studentCourses = await _studentAccountService.GetStudentCoursesByUserIdAsync(userId);
            return studentCourses.Select(x =>
            {
                var lesson = _mapper.Map<StudentCourse, Response.LessonsAndMarks>(x);
                lesson.Marks = x.Assessments
                    .Select(z => _mapper.Map<Assessment, Response.Mark>(z))
                    .ToList();
                return lesson;
            }).ToList();
        }
    }
}
