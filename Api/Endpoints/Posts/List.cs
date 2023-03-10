using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class List: BaseAsyncEndpoint
        .WithRequest<Request.List>
        .WithResponse<List<Response.List>>
    {
        private readonly SsnDbContext _context;
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly PostsService _service;
        private readonly IMapper _mapper;

        public List(SsnDbContext context, IMapper mapper, PostsService service, 
            UserManager<ApplicationUser> userManager, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _mapper = mapper;
            _service = service;
            _userManager = userManager;
            _currentUserAccessor = currentUserAccessor;
        }
        
        [JwtAuthorize]
        [HttpGet("api/posts")]
        [SwaggerOperation(
            Summary = "List of posts",
            Description = "List of posts",
            OperationId = "posts.list",
            Tags = new []{"Posts"})]
        public override async Task<ActionResult<List<Response.List>>> HandleAsync([FromQuery] Request.List request, 
            CancellationToken cancellationToken = new())
        {
            var query = _service.GetAllData();

            if (request.Tags != null && request.Tags.Any())
            {
                var tags = _context.Tags.Where(x => request.Tags.Contains(x.Name));
                if (tags.Any()) query = query.Where(x => x.Tags.Intersect(tags).Any());
                else return default;
            }

            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user != null) query = query.Where(x => x.User == user);
                else return default;
            }
            
            var currentUserId = _currentUserAccessor.GetCurrentUserId();
            var posts = query
                .OrderByDescending(x => x.CreatedAt)
                .Skip(request.Offset ?? 0)
                .Take(request.Limit ?? 20)
                .AsEnumerable()
                // ???????????? ?????? ?????????????????????????? ProjectTo: ?????????????????? ???????????????? LikesCount
                .Select(x =>
                {
                    var model = _mapper.Map<Response.List>(x);
                    model.IsCurrentUserLiked = x.IsUserLikedPost(currentUserId);
                    return model;
                })
                .ToList();

            return posts;
        }
    }
}
