using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class Posts : BaseAsyncEndpoint
        .WithRequest<Request.CreatePost>
        .WithResponse<Response.CreatePost>
    {
        private readonly PostService _postService;
        private readonly ILogger<Posts> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public Posts(ILogger<Posts> logger, PostService postService, 
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _postService = postService;
            _userManager = userManager;
        }
        
        [HttpPost("api/create-post")]
        [SwaggerOperation(
            Summary = "Create a post",
            Description = "Create a post",
            OperationId = "post.create",
            Tags = new []{"Post.Create"})]
        
        public override async Task<ActionResult<Response.CreatePost>> HandleAsync(
            Request.CreatePost request, CancellationToken cancellationToken = new ())
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogWarning("Creating post starts");

            var post = _mapper.Map<Post>(request);

            var createdPost = await _postService.CreatePostAsync(
                request.Body, request.IsActive, request.IsDraft, request.UserID);
            if (createdPost == null)
                return BadRequest(Result.PostNotCreated);
            var userFound = await _userManager.FindByIdAsync(request.UserID);
            if (userFound == null)
                return BadRequest(Result.UserNotFound);

            var user = await _userManager.FindByIdAsync(request.UserID);
            var username = user.UserName;
            
            post.CreatedAt = DateTime.UtcNow;
            post.Body = createdPost.Body;
            post.IsActive = createdPost.IsActive;
            post.IsDraft = createdPost.IsDraft;
            post.UserId = createdPost.UserId;
            post.User = user;
            
            _logger.LogWarning($"User {User} created post");
            
            stopWatch.Stop();
            _logger.LogWarning("Creating post ended. {Milliseconds} ms elapsed", stopWatch.Elapsed.Milliseconds);

            return new Response.CreatePost()
            {
                Username = username,
                PostBody = createdPost.Body,
            };
        }

        // public async Task<bool> CheckIsPostCreated(Request.CreatePost request)
        // {
        //     //check is post is created
        //     
        //     return true;
        // }
    }
}