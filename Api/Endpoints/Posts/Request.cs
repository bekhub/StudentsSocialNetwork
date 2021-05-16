using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints.Posts
{
    public class Request
    {
        public class CreatePost
        {
            [Required]
            public string Body { get; set; }

            public List<IFormFile> PostPictures { get; set; }
            
            public List<string> Tags { get; set; }
        }
    }
}