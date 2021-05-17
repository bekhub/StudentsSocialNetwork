using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Comments
{
    public class Like : BaseAsyncEndpoint
        .WithRequest<Request.Like>
        .WithResponse<Result>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;

        public Like(ICurrentUserAccessor currentUserAccessor, SsnDbContext context)
        {
            _currentUserAccessor = currentUserAccessor;
            _context = context;
        }

        [JwtAuthorize(RoleConstants.USER)]
        [HttpPatch("api/comments/like")]
        [SwaggerOperation(
            Summary = "Like comment",
            Description = "Like comment",
            OperationId = "comments.like",
            Tags = new[] {"Comments"})]
        public override async Task<ActionResult<Result>> HandleAsync(Request.Like request, 
            CancellationToken cancellationToken = new ())
        {
            var userId = _currentUserAccessor.GetCurrentUserId();
            var comment = await _context.Comments
                .Include(x => x.CommentLikes)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (comment == null) return BadRequest(Result.From(Resource.PostNotFound));

            var commentLike = comment.CommentLikes.SingleOrDefault(x => x.UserId == userId);

            if (commentLike == null) comment.CommentLikes.Add(new CommentLike {Comment = comment, UserId = userId});
            else comment.CommentLikes.Remove(commentLike);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.From(Resource.CommentLikeSuccess);
        }
    }
}
