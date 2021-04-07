namespace Api.Endpoints.Auth
{
    public class Response
    {
        public class Authenticate
        {
            public string Username { get; set; }
            
            public string JwtToken { get; set; }

            public string RefreshToken { get; set; }
        }
        
        public class CheckStudent
        {
            public bool IsExist { get; set; }

            public string Message { get; set; }
        }

        public record VerifyEmail(bool IsVerified);
    }
}
