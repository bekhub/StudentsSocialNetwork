using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class ListPost: BaseAsyncEndpoint
        .WithoutRequest
        .WithResponse<Response.ListPosts>
    {
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;

        public ListPost(SsnDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        
        [HttpGet("api/list-post")]
        [SwaggerOperation(
            Summary = "Show all posts",
            Description = "Show all posts",
            OperationId = "post.list",
            Tags = new []{"Posts"})]
        
        public override async Task<ActionResult<Response.ListPosts>> HandleAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            List<Post> list = new List<Post>();

            list = await _context.Posts
                .OrderByDescending(x =>x.UpdatedAt)
                // .Include(x => x.Tags)
                // .Include(x => x.User)
                .ToListAsync(cancellationToken);

            return new Response.ListPosts()
            {
                Posts = list
            };
        }
    }
}