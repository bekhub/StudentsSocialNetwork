using System.Collections.Generic;

namespace Api.Endpoints.Posts
{
    public class Response
    {
        public class CreatePost
        {
            public string Username { get; set; }
        
            public string PostBody { get; set; }    
            
            public List<string> Tags { get; set; }
        }
    }
}