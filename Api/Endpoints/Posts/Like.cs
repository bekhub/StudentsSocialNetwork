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

namespace Api.Endpoints.Posts
{
    public class Like : BaseAsyncEndpoint
        .WithRequest<Request.Like>
        .WithResponse<Response.Like>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;

        public Like(ICurrentUserAccessor currentUserAccessor, SsnDbContext context)
        {
            _currentUserAccessor = currentUserAccessor;
            _context = context;
        }

        [JwtAuthorize(RoleConstants.USER)]
        [HttpPatch("api/posts/like")]
        [SwaggerOperation(
            Summary = "Like post",
            Description = "Like post",
            OperationId = "posts.like",
            Tags = new[] {"Posts"})]
        public override async Task<ActionResult<Response.Like>> HandleAsync(Request.Like request,
            CancellationToken cancellationToken = new())
        {
            var userId = _currentUserAccessor.GetCurrentUserId();
            var post = await _context.Posts
                .Include(x => x.PostLikes)
                .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (post == null) return Result.BadRequest(Resource.PostNotFound);

            var postLike = post.PostLikes.SingleOrDefault(x => x.UserId == userId);

            if (postLike == null)
            {
                postLike = new PostLike {Post = post, UserId = userId, IsLiked = true};
                post.PostLikes.Add(postLike);
            }
            else postLike.IsLiked = !postLike.IsLiked;
            

            await _context.SaveChangesAsync(cancellationToken);

            return new Response.Like(postLike.IsLiked);
        }
    }
}
