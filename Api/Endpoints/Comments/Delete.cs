using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Comments
{
    public class Delete : BaseAsyncEndpoint
        .WithRequest<Request.Delete>
        .WithResponse<Result>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;

        public Delete(ICurrentUserAccessor currentUserAccessor, SsnDbContext context)
        {
            _currentUserAccessor = currentUserAccessor;
            _context = context;
        }

        [JwtAuthorize(RoleConstants.USER)]
        [HttpDelete("api/comments")]
        [SwaggerOperation(
            Summary = "Delete comment",
            Description = "Delete comment",
            OperationId = "comments.delete",
            Tags = new[] {"Comments"})]
        public override async Task<ActionResult<Result>> HandleAsync(Request.Delete request,
            CancellationToken cancellationToken = new())
        {
            var comment = await _context.Comments
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (comment == null) return BadRequest(Result.From(Resource.CommentNotFound));
            var userId = _currentUserAccessor.GetCurrentUserId();
            if (comment.UserId != userId) return BadRequest(Result.From(Resource.CommentUserError));

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync(cancellationToken);

            return Result.From(Resource.CommentDeleted);
        }
    }
}
