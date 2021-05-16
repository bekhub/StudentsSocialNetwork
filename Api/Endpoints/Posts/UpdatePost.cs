using System;
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
    public class UpdatePost : BaseAsyncEndpoint
        .WithRequest<Request.UpdatePost>
        .WithoutResponse
    {
        private readonly SsnDbContext _context;

        public UpdatePost(SsnDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("api/update-post")]
        [SwaggerOperation(
            Summary = "Update post",
            Description = "Update post",
            OperationId = "post.update",
            Tags = new[] {"Posts"})]
        
        public override async Task<ActionResult> HandleAsync(Request.UpdatePost request, CancellationToken cancellationToken = new CancellationToken())
        {
            var post = await _context.Posts
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
                return BadRequest(Result.PostNotFound);

            post.Body = request.Body;
            post.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}