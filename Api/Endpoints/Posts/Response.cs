namespace Api.Endpoints.Posts
{
    public class Response
    {
        public class CreatePost
        {
            public string Username { get; set; }
        
            public string PostBody { get; set; }    
        }
    }
}