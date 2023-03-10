using System;
using System.Collections.Generic;

namespace Api.Endpoints.Posts
{
    public class Response
    {
        public class Create
        {
            public int Id { get; set; }
            
            public string Username { get; set; }
        
            public string Body { get; set; }    
            
            public List<string> Tags { get; set; }
            
            public List<string> Pictures { get; set; }
        }

        public class Update : Create { }

        public class Details : Create
        {
            public string UserPictureUrl { get; set; }

            public bool IsCurrentUserLiked { get; set; }

            public int LikesCount { get; set; }

            public int CommentsCount { get; set; }
            
            public DateTime UpdatedAt { get; set; }
        }
        
        public class List : Details { }

        public record Like(bool IsLiked);

        public class Likes
        {
            public string UserId { get; init; }
            
            public string Username { get; init; }

            public string ProfilePictureUrl { get; set; }
        }
    }
}
