using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using Core.Entities;
using Core.Interfaces;
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
        private readonly IFileSystem _fileSystem;

        private const string FOLDER = "post_pictures";

        public DeletePost(SsnDbContext context, IFileSystem fileSystem)
        {
            _context = context;
            _fileSystem = fileSystem;
        }
        
        [HttpDelete("api/delete-post")]
        [SwaggerOperation(
            Summary = "Delete post",
            Description = "Delete post",
            OperationId = "post.delete",
            Tags = new[] {"Posts"})]

        public override async Task<ActionResult> HandleAsync(Request.DeletePost request, CancellationToken cancellationToken = new CancellationToken())
        {
            var post = await _context.Posts
                .Where(p => p.Id == request.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (post == null)
                return BadRequest(Result.PostNotFound);

            foreach (var tag in post.Tags)
            {
                foreach (var postTag in tag.PostTags)
                {
                    if(postTag.PostId == post.Id)
                        _context.Tags.Remove(tag);
                }
            }
            foreach (var pics in post.Pictures)
            {
                _fileSystem.DeletePicture(_fileSystem.GeneratePictureName(pics.Url), FOLDER);
            }
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok();
        }
    }
}