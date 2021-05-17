using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class CreatePost : BaseAsyncEndpoint
        .WithRequest<Request.Create>
        .WithResponse<Response.Create>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;
        private readonly PostsService _service;

        public CreatePost(SsnDbContext context, IMapper mapper, 
            ICurrentUserAccessor currentUserAccessor, PostsService service)
        {
            _context = context;
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
            _service = service;
        }
        
        [JwtAuthorize(RoleConstants.USER)]
        [HttpPost("api/posts")]
        [SwaggerOperation(
            Summary = "Create a post",
            Description = "Create a post",
            OperationId = "posts.create",
            Tags = new []{"Posts"})]
        public override async Task<ActionResult<Response.Create>> HandleAsync([FromForm] Request.Create request,
            CancellationToken cancellationToken = new())
        {
            var post = _mapper.Map<Post>(request);
            post.User = await _currentUserAccessor.GetCurrentUserAsync(cancellationToken);
            post.Tags = _service.GetTags(request.Tags?.ToHashSet());
            post.Pictures = _service.GetPictures(request.Pictures);

            await _context.Posts.AddAsync(post, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            
            return _mapper.Map<Response.Create>(post);
        }
    }
}
