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
        
        public class CheckStudent
        {
            public string StudentNumber { get; set; }

            public string StudentPassword { get; set; }
        }
        
        public class Register
        {
            [Required]
            public string StudentNumber { get; set; }
            
            [Required]
            public string StudentPassword { get; set; }

            [Required]
            public string Username { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string Password { get; set; }
            
            public IFormFile ProfilePicture { get; set; }
        }
        
        public class VerifyEmail
        {
            public string Email { get; set; }
            
            public string Token { get; set; }
        }
    }
}
