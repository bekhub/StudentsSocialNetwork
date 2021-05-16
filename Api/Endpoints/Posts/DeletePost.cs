using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class DeletePost : BaseAsyncEndpoint
        .WithRequest<Request.DeletePost>
        .WithoutResponse
    {
        private readonly SsnDbContext _context;

        public DeletePost(SsnDbContext context)
        {
            _context = context;
        }
        
        [HttpPost("api/delete-post")]
        [SwaggerOperation(
            Summary = "Delete post",
            Description = "Delete post",
            OperationId = "post.delete",
            Tags =  new[] {"Posts"})]

        public override async Task<ActionResult> HandleAsync(Request.DeletePost request, CancellationToken cancellationToken = new CancellationToken())
        {
            var post = await _context.Posts
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
                return BadRequest(Result.PostNotFound);

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}