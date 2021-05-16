using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Api.Helpers;
using Api.Helpers.Extensions;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;

namespace Api.Endpoints.Posts
{
    public class CreatePost : BaseAsyncEndpoint
        .WithRequest<Request.CreatePost>
        .WithResponse<Response.CreatePost>
    {
        private readonly ICurrentUserAccessor _currentUserAccessor;
        private readonly SsnDbContext _context;
        private readonly PostService _postService;
        private readonly ILogger<CreatePost> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IFileSystem _fileSystem;
        
        private const string FOLDER = "post_pictures";

        public CreatePost(SsnDbContext context, ILogger<CreatePost> logger, PostService postService, 
            IMapper mapper, UserManager<ApplicationUser> userManager, ICurrentUserAccessor currentUserAccessor, IFileSystem fileSystem)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _postService = postService;
            _userManager = userManager;
            _currentUserAccessor = currentUserAccessor;
            _fileSystem = fileSystem;
        }
        
        [HttpPost("api/create-post")]
        [SwaggerOperation(
            Summary = "Create a post",
            Description = "Create a post",
            OperationId = "post.create",
            Tags = new []{"Posts"})]
        
        public override async Task<ActionResult<Response.CreatePost>> HandleAsync(
            [FromForm] Request.CreatePost request, CancellationToken cancellationToken = new ())
        {
            var post = _mapper.Map<Posts.Request.CreatePost, Post>(request);
            post.User = await _currentUserAccessor.GetCurrentUserAsync(cancellationToken);

            if (post.User != null)
            {
                var user = post.User;
                var tags = request.Tags;
                var postPictures = request.PostPictures;

                post.CreatedAt = DateTime.UtcNow;
                post.UpdatedAt = DateTime.UtcNow;
                post.IsActive = true;
                post.IsDraft = false;
                post.Body = request.Body;
                post.UserId = user.Id;
                post.User = user;
            
                await _context.Posts.AddAsync(post, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                
                var TagsList = new List<Tag>();
                if (tags == null)
                    TagsList = null;
                else
                {
                    foreach (var tag in tags)
                    {
                        var newTag = new Tag();
                        newTag.Name = tag;
                        // need to find to which Post it belongs
                        var NewPostTag = new PostTag();
                        NewPostTag.Post = post;
                        NewPostTag.PostId = post.Id;
                        NewPostTag.Tag = newTag;
                        NewPostTag.TagId = newTag.Id;
                        var NewPostTagList = new List<PostTag>();
                        NewPostTagList.Add(NewPostTag);
                        newTag.PostTags = NewPostTagList;
                        TagsList.Add(newTag);
                    }
                }
                
                post.Tags = TagsList;
                
                var PostPicsList = new List<PostPicture>();
                var PostPicsUrls = new List<string>();
            
                if (postPictures == null)
                    PostPicsList = null;
                else
                {
                    foreach (var postPics in postPictures)
                    {
                        // need to find to which Post it belongs
                        var newPic = new PostPicture();
                        newPic.Url = await MakePictureUrlAsync(postPics);
                        
                        PostPicsUrls.Add(newPic.Url);
                        PostPicsList.Add(newPic);
                    }    
                }
                post.Pictures = PostPicsList;
                
                await _context.SaveChangesAsync(cancellationToken);
                
                return new Response.CreatePost()
                {
                    Username = user.UserName,
                    PostBody = post.Body,
                    Tags = request.Tags,
                    Pics = PostPicsUrls
                };
            }

            return BadRequest(Result.NotAuthorized);
        }
        
        private async Task<string> MakePictureUrlAsync(IFormFile file)
        {
            if(file is not {Length: > 0}) return  string.Empty;

            var picName = _fileSystem.GeneratePictureName(file.FileName);
            var picture = file.ToArray();

            if (!await _fileSystem.SavePictureAsync(picName, picture, FOLDER))
                return string.Empty;

            return _fileSystem.MakePictureUrl(picName, FOLDER);
        }
    }
}