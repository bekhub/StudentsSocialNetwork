using System.Collections.Generic;
using System.Linq;
using Api.Configuration;
using Ardalis.ApiEndpoints;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class Likes : BaseEndpoint
        .WithRequest<Request.Likes>
        .WithResponse<List<Response.Likes>>
    {
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;

        public Likes(SsnDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [JwtAuthorize]
        [HttpGet("api/posts/likes")]
        [SwaggerOperation(
            Summary = "Checks either user liked post or not",
            Description = "Checks either user liked post or not",
            OperationId = "posts.isUserLiked",
            Tags = new []{"Posts"})]
        public override ActionResult<List<Response.Likes>> Handle([FromQuery] Request.Likes request)
        {
            return _context.PostLikes
                .Include(x => x.User)
                .Where(x => x.PostId == request.PostId && x.IsLiked)
                .ProjectTo<Response.Likes>(_mapper.ConfigurationProvider)
                .ToList();
        }
    }
}
