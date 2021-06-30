using System;
using System.Collections.Generic;

namespace Api.Endpoints.Comments
{
    public class Response
    {
        public record Create(int Id);
        
        public class ListComment
        {
            public int Id { get; set; }

            public string Username { get; set; }
            
            public string ProfilePictureUrl { get; set; }
            
            public string Body { get; set; }

            public int LikesCount { get; set; }
            
            public int CommentsCount { get; set; }

            public DateTime CreatedAt { get; set; }

            public List<ListComment> Replies { get; set; }
        }
    }
}
