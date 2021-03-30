namespace Api.Endpoints.Auth
{
    public class Response
    {
        public class Authenticate
        {
            public string Username { get; set; } = string.Empty;
            
            public string Token { get; set; } = string.Empty;
        }
    }
}
