using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Api.Configuration;
using Api.Helpers;
using Api.Resources;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Comments
{
    public class List : BaseAsyncEndpoint
        .WithRequest<int>
        .WithResponse<List<Response.ListComment>>
    {
        private readonly SsnDbContext _context;
        private readonly IMapper _mapper;

        public List(SsnDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [JwtAuthorize(RoleConstants.USER)]
        [HttpGet("api/comments/{postId:int}")]
        [SwaggerOperation(
            Summary = "Comments list",
            Description = "Comments list",
            OperationId = "comments.list",
            Tags = new[] {"Comments"})]
        public override async Task<ActionResult<List<Response.ListComment>>> HandleAsync(int postId, 
            CancellationToken cancellationToken = new())
        {
            if (!await _context.Posts.AnyAsync(x => x.Id == postId, cancellationToken)) 
                return BadRequest(Result.From(Resource.PostNotFound));

            var comments = _context.Comments
                .Include(x => x.CommentLikes)
                .Include(x => x.Replies)
                .ThenInclude(x => x.CommentLikes)
                .Include(x => x.User)
                .Where(x => x.PostId == postId)
                .AsEnumerable()
                .Select(x =>
                {
                    var comment = _mapper.Map<Response.ListComment>(x);
                    comment.Replies = x.Replies
                        .Select(c => _mapper.Map<Response.ListComment>(c))
                        .ToList();
                    return comment;
                });

            return comments.ToList();
        }
    }
}
