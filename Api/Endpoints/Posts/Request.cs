using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints.Posts
{
    public class Request
    {
        public class Create
        {
            [Required]
            public string Body { get; set; }

            public List<IFormFile> Pictures { get; set; }
            
            public List<string> Tags { get; set; }
        }

        public class Update
        {
            public int Id { get; set; }

            public string Body { get; set; }

            /// <summary>
            /// Pictures that not deleted
            /// </summary>
            public List<string> PostPictureUrls { get; set; }
            
            public List<IFormFile> NewPictures { get; set; }

            public List<string> Tags { get; set; }
        }
        
        public class List
        {
            public HashSet<string> Tags { get; set; }

            public string Username { get; set; }

            public int? Limit { get; set; }
            
            public int? Offset { get; set; }

            public bool IsFeed { get; set; } = false;
        }

        public record Delete(int Id);

        public record Details(int Id);

        public record Like(int Id);

        public record Likes(int PostId);
    }
}
