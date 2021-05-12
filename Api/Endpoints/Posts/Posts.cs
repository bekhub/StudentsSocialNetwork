using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class Posts : BaseAsyncEndpoint
        .WithRequest<Request.CreatePost>
        .WithoutResponse
    {
        private readonly PostService _postService;
        private readonly ILogger<Posts> _logger;
        private readonly IMapper _mapper;

        public Posts(ILogger<Posts> logger, PostService postService, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _postService = postService;
        }
        
        [HttpPost("api/create/post")]
        [SwaggerOperation(
            Summary = "Create a post",
            Description = "Create a post",
            OperationId = "post.create",
            Tags = new []{"Post.Create"})]
        
        public override async Task<ActionResult> HandleAsync(Request.CreatePost request, CancellationToken cancellationToken = new ())
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogWarning("Creating post starts");

            var post = _mapper.Map<Post>(request);
            post.CreatedAt = DateTime.UtcNow;

            var createdPost = await _postService.CreatePostAsync(request.Body, request.User);
            if (createdPost == null)
                return BadRequest(Result.PostNotCreated);

            post.Body = createdPost.Body;
            post.User = createdPost.User;
            
            _logger.LogWarning($"User {User} created post", request.User);
            
            stopWatch.Stop();
            _logger.LogWarning("Creating post ended. {Milliseconds} ms elapsed", stopWatch.Elapsed.Milliseconds);

            return Ok(Result.PostCreated);
        }

        public async Task<bool> CheckIsPostCreated(Request.CreatePost request)
        {
            //check is post is created
            
            return true;
        }
    }
}