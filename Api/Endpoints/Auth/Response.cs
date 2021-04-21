﻿namespace Api.Endpoints.Auth
{
    public class Response
    {
        public class Authenticate
        {
            public string Username { get; set; }

            public string UserId { get; set; }
            
            public string JwtToken { get; set; }

            public string RefreshToken { get; set; }
        }
    }
}
