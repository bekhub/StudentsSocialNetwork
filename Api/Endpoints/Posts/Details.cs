using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class Details : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response.Details>
    {
        private readonly SsnDbContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly IMapper _mapper;

        public Details(SsnDbContext context, IMapper mapper, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _mapper = mapper;
            _currentUserAccessor = currentUserAccessor;
        }
        
        [JwtAuthorize]
        [HttpGet("api/posts/{id:int}")]
        [SwaggerOperation(
            Summary = "Post details",
            Description = "Post details",
            OperationId = "posts.details",
            Tags = new[] {"Posts"})]
        public override async Task<ActionResult<Response.Details>> HandleAsync(int id, CancellationToken cancellationToken = new())
        {
            var post = await _context.Posts
                .Where(x => x.IsActive && x.Id == id)
                .Include(x => x.Pictures)
                .Include(x => x.Tags)
                .Include(x => x.User)
                .Include(x => x.PostLikes)
                .Include(x => x.Comments)
                    .ThenInclude(x => x.Replies)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (post == null) return BadRequest(Result.From(Resource.PostNotFound));
            
            var userId = _currentUserAccessor.GetCurrentUserId();
            var details = _mapper.Map<Response.Details>(post);
            details.IsCurrentUserLiked = post.IsUserLikedPost(userId);

            return details;
        }
    }
}
