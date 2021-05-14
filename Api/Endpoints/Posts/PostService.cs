using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Helpers.Extensions;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Api.Endpoints.Posts
{
    public class PostService
    {
        private IFileSystem _fileSystem;
        private string FOLDER;
        public async Task<Post> CreatePostAsync(string body, bool isDraft, string userId, List<string> tags, 
            List<IFormFile> postPictures, IFileSystem fileSystem, string _FOLDER)
        {

            _fileSystem = fileSystem;
            FOLDER = _FOLDER;

            var TagsList = new List<Tag>();
            foreach (var tag in tags)
            {
                var newTag = new Tag();
                newTag.Name = tag;
                newTag.PostTags = new List<PostTag>();
                TagsList.Add(newTag);
            }

            var PostPicsList = new List<PostPicture>();
            foreach (var postPics in postPictures)
            {
                var newPic = new PostPicture();
                newPic.Url = await MakePictureUrlAsync(postPics);
                
                PostPicsList.Add(newPic);
            }
            
            return new Post
            {
                Body = body,
                IsDraft = isDraft,
                Pictures = PostPicsList,
                UserId = userId,
                Tags = TagsList
            };
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