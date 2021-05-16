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
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class Create : BaseAsyncEndpoint
        .WithRequest<Request.CreatePost>
        .WithResponse<Response.CreatePost>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;
        private readonly PostService _postService;
        private readonly ILogger<Create> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public Create(SsnDbContext context, ILogger<Create> logger, PostService postService, 
            IMapper mapper, UserManager<ApplicationUser> userManager, ICurrentUserAccessor currentUserAccessor)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _postService = postService;
            _userManager = userManager;
            _currentUserAccessor = currentUserAccessor;
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
            var post = _mapper.Map<Post>(request);
            
            var createdPost = await _postService.CheckPostAsync(request.Body, 
                request.Tags, request.PostPictures);
            if (createdPost == null)
                return BadRequest(Result.PostNotCreated);
            
            post.User = await _currentUserAccessor.GetCurrentUserAsync(cancellationToken);

            var user = post.User;
            if (user != null)
            {
                var username = user.UserName;
                var userFound = await _userManager.FindByIdAsync(user.Id);
                if (userFound == null)
                    return BadRequest(Result.UserNotFound);

                post.CreatedAt = DateTime.UtcNow;
                post.UpdatedAt = DateTime.UtcNow;
                post.IsActive = true;
                post.IsDraft = false;
                post.Body = createdPost.Body;
                post.UserId = createdPost.UserId;
                post.User = user;
            
                await _context.Posts.AddAsync(post, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response.CreatePost()
                {
                    Username = username,
                    PostBody = createdPost.Body,
                    Tags = request.Tags
                };
            }

            return BadRequest(Result.NotAuthorized);
        }
    }
}