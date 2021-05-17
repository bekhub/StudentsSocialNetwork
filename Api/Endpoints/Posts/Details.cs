using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class DetailedPost : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<Response.Details>
    {
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;

        public DetailedPost(SsnDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [JwtAuthorize]
        [HttpGet("api/posts/{id:int}")]
        [SwaggerOperation(
            Summary = "Post details",
            Description = "Post details",
            OperationId = "posts.details",
            Tags = new[] {"Posts"})]
        public override async Task<ActionResult<Response.Details>> HandleAsync(int id, CancellationToken cancellationToken = new CancellationToken())
        {
            var post = await _context.Posts
                .Where(x => x.IsActive && x.Id == id)
                .Include(x => x.Pictures)
                .Include(x => x.Tags)
                .Include(x => x.User)
                .Include(x => x.PostLikes)
                .Include(x => x.Comments)
                .ProjectTo<Response.Details>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);
            
            if (post == null) return BadRequest(Result.From(DefaultResource.PostNotFound));

            return post;
        }
    }
}
