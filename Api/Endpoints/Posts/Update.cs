using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class Update : BaseAsyncEndpoint
        .WithRequest<Request.Update>
        .WithResponse<Response.Update>
    {
        private readonly SsnDbContext _context;
        private readonly PostsService _service;
        private readonly IMapper _mapper;

        public Update(SsnDbContext context, PostsService service, IMapper mapper)
        {
            _context = context;
            _service = service;
            _mapper = mapper;
        }
        
        [JwtAuthorize(RoleConstants.USER)]
        [HttpPut("api/posts")]
        [SwaggerOperation(
            Summary = "Update post",
            Description = "Update post",
            OperationId = "posts.update",
            Tags = new[] {"Posts"})]
        public override async Task<ActionResult<Response.Update>> HandleAsync([FromForm] Request.Update request, 
            CancellationToken cancellationToken = new())
        {
            var post = await _service.ByIdAsync(request.Id, cancellationToken);

            if (post == null) return BadRequest(Result.From(Resource.PostNotFound));

            post.Body = request.Body;
            post.UpdatedAt = DateTime.UtcNow;
            post.Tags = _service.GetTags(request.Tags?.ToHashSet());
            post.Pictures = UpdatedPictures(request, post.Pictures);

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<Response.Update>(post);
        }

        private List<PostPicture> UpdatedPictures(Request.Update request, IReadOnlyCollection<PostPicture> postPictures)
        {
            var newPictures = _service.GetPictures(request.NewPictures);
            var postPictureUrls = request.PostPictureUrls?.ToHashSet() ?? new HashSet<string>();

            var pictureUrlsToDelete = postPictures
                .Where(x => !postPictureUrls.Contains(x.Url))
                .Select(x => x.Url);
            _service.DeletePictures(pictureUrlsToDelete);
            
            newPictures.AddRange(postPictures.Where(x => postPictureUrls.Contains(x.Url)));
            return newPictures;
        }
    }
}
