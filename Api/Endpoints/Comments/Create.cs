using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Comments
{
    public class Create : BaseAsyncEndpoint
        .WithRequest<Request.Create>
        .WithResponse<Response.Create>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;

        public Create(ICurrentUserAccessor currentUserAccessor, SsnDbContext context, IMapper mapper)
        {
            _currentUserAccessor = currentUserAccessor;
            _context = context;
            _mapper = mapper;
        }

        [JwtAuthorize(RoleConstants.USER)]
        [HttpPost("api/comments")]
        [SwaggerOperation(
            Summary = "Create a comment",
            Description = "Create a comment",
            OperationId = "comments.create",
            Tags = new []{"Comments"})]
        public override async Task<ActionResult<Response.Create>> HandleAsync(Request.Create request, 
            CancellationToken cancellationToken = new())
        {
            if (string.IsNullOrWhiteSpace(request.Body)) return BadRequest(Result.From(Resource.BodyRequired)); 
            if (!IsValidTarget(request)) return BadRequest(Result.From(Resource.CommentTargetError));
            if (!IsTargetExists(request)) return BadRequest(Result.From(Resource.TargetNotFound));
            if (!IsValidNesting(request)) return BadRequest(Result.From(Resource.NestingError));
            
            var comment = _mapper.Map<Comment>(request);
            comment.UserId = _currentUserAccessor.GetCurrentUserId();
            await _context.AddAsync(comment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return new Response.Create(comment.Id);
        }

        private static bool IsValidTarget(Request.Create request)
        {
            return (request.PostId != null || request.TargetId != null) 
                   && (request.PostId == null || request.TargetId == null);
        }

        private bool IsTargetExists(Request.Create request)
        {
            return request.PostId.HasValue 
                ? _context.Posts.Any(x => x.Id == request.PostId) 
                : _context.Comments.Any(x => x.Id == request.TargetId);
        }

        private bool IsValidNesting(Request.Create request)
        {
            if (!request.TargetId.HasValue) return true;
            var target = _context.Comments.Single(x => x.Id == request.TargetId);
            return !target.TargetId.HasValue;
        }
    }
}
