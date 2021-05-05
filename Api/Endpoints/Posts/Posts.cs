using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Api.Endpoints.Registration;
using Api.Helpers;
using Api.Helpers.Extensions;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
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
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<Posts> _logger;
        private readonly IMapper _mapper;

        private const string FOLDER = "post_pictures";

        public Posts(IFileSystem fileSystem, ILogger<Posts> logger, 
            PostService postService, IMapper mapper)
        {
            _fileSystem = fileSystem;
            _logger = logger;
            _mapper = mapper;
            _postService = postService;
        }
        
        [HttpPost("api/createPost")]
        [SwaggerOperation(
            Summary = "Create a post",
            Description = "Create a post",
            OperationId = "post.create",
            Tags = new []{"Post.Create"})]
        
        public override async Task<ActionResult> HandleAsync(Request.CreatePost request, CancellationToken cancellationToken = new CancellationToken())
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            _logger.LogWarning("Creating post starts");

            var post = _mapper.Map<Post>(request);
            post.CreatedAt = DateTime.UtcNow;
            // post.Pictures = await MakePictureUrlAsync(request.PostPic);

            
            throw new System.NotImplementedException();
        }

        public async Task<bool> CheckIsPostCreated(Request.CreatePost createPost)
        {
            
            return true;
        }

        private async Task<string> MakePictureUrlAsync(IFormFile file)
        {
            if (file is not {Length: > 0}) return string.Empty;

            var picName = _fileSystem.GeneratePictureName(file.FileName);
            var picture = file.ToArray();

            if (!await _fileSystem.SavePictureAsync(picName, picture, FOLDER))
                return string.Empty;

            return _fileSystem.MakePictureUrl(picName, FOLDER);
        }
    }
}