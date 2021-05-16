using System.Collections.Generic;
using Core.Entities;

namespace Api.Endpoints.Posts
{
    public class Response
    {
        public class CreatePost
        {
            public string Username { get; set; }
        
            public string PostBody { get; set; }    
            
            public List<string> Tags { get; set; }
            
            public List<string> Pics { get; set; }
        }
        
        public class ShowPost
        {
            public Post Post { get; set; }
        }
        
        public class ListPosts
        {
            public List<Post> Posts { get; set; }
        }
    }
}