using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class DetailedPost : BaseAsyncEndpoint
        .WithRequest<Request.ShowPost>
        .WithResponse<Response.ShowPost>
    {
        private readonly SsnDbContext _context;

        public DetailedPost(SsnDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("api/show-post")]
        [SwaggerOperation(
            Summary = "Show post",
            Description = "Show post",
            OperationId = "post.show",
            Tags = new[] {"Posts"})]
        
        public override async Task<ActionResult<Response.ShowPost>> HandleAsync(Request.ShowPost request, CancellationToken cancellationToken = new CancellationToken())
        {
            var post = await _context.Posts
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);
            
            if(post == null)
                return BadRequest(Result.PostNotFound);

            return new Response.ShowPost()
            {
                Post = post
            };
        }
    }
}