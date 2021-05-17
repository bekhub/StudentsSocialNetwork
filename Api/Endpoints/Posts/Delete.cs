using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class DeletePost : BaseAsyncEndpoint
        .WithRequest<Request.Delete>
        .WithResponse<Result>
    {
        private readonly SsnDbContext _context;
        private readonly PostsService _service;

        public DeletePost(SsnDbContext context, PostsService service)
        {
            _context = context;
            _service = service;
        }
        
        [JwtAuthorize(RoleConstants.USER)]
        [HttpDelete("api/posts")]
        [SwaggerOperation(
            Summary = "Delete post",
            Description = "Delete post",
            OperationId = "posts.delete",
            Tags = new[] {"Posts"})]
        public override async Task<ActionResult<Result>> HandleAsync(Request.Delete request, 
            CancellationToken cancellationToken = new())
        {
            var post = await _service.ByIdAsync(request.Id, cancellationToken);

            if (post == null) return BadRequest(Result.From(DefaultResource.PostNotFound));

            if (post.Pictures.Any())
                _service.DeletePictures(post.Pictures.Select(x => x.Url));
            
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(Result.From(DefaultResource.PostDeleted));
        }
    }
}
