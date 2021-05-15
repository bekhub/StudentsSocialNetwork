using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
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
        private readonly IFileSystem _fileSystem;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        // private readonly UserManager<Post> _postManager;

        private const string FOLDER = "profiles_pictures";

        public Posts(ILogger<Posts> logger, PostService postService, IFileSystem fileSystem, 
            IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _mapper = mapper;
            _fileSystem = fileSystem;
            _postService = postService;
            _userManager = userManager;
            // _postManager = postManager;
        }
        
        [HttpPost("api/create-post")]
        [SwaggerOperation(
            Summary = "Create a post",
            Description = "Create a post",
            OperationId = "post.create",
            Tags = new []{"Post.Create"})]
        
        public override async Task<ActionResult<Response.CreatePost>> HandleAsync(
            [FromForm] Request.CreatePost request, CancellationToken cancellationToken = new ())
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogWarning("Creating post starts");

            var post = _mapper.Map<Post>(request);

            var createdPost = await _postService.CreatePostAsync(request.Body, request.UserID, 
                request.Tags, request.PostPictures, _fileSystem, FOLDER);
            if (createdPost == null)
                return BadRequest(Result.PostNotCreated);
            var userFound = await _userManager.FindByIdAsync(request.UserID);
            if (userFound == null)
                return BadRequest(Result.UserNotFound);

            var user = await _userManager.FindByIdAsync(request.UserID);
            var username = user.UserName;

            post.Pictures = new List<PostPicture>();
            post.CreatedAt = DateTime.UtcNow;
            post.IsActive = true;
            post.IsDraft = false;
            post.Body = createdPost.Body;
            post.UserId = createdPost.UserId;
            post.User = user;

            // var result = await _userManager.CreateAsync(user, user.PasswordHash);
            // if (!result.Succeeded) return BadRequest(Result.PostError);
            
            _logger.LogWarning($"User {user} created post");
            
            stopWatch.Stop();
            _logger.LogWarning("Creating post ended. {Milliseconds} ms elapsed", stopWatch.Elapsed.Milliseconds);

            return new Response.CreatePost()
            {
                Username = username,
                PostBody = createdPost.Body,
                Tags = request.Tags
            };
        }
    }
}