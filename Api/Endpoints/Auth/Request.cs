using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints.Auth
{
    public class Request
    {
        public class Authenticate
        {
            public string Username { get; set; }

            public string StudentNumber { get; set; }
            
            public string Password { get; set; }
        }
    }
}
