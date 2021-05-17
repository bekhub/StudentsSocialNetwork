using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Api.Endpoints.Registration
{
    public class Request
    {
        public record CheckStudent(string StudentNumber, string StudentPassword);
        
        public class Register
        {
            [Required]
            public string StudentNumber { get; set; }
            
            [Required]
            [DataType(DataType.Password)]
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

        public record VerifyEmail(string Email, string Token);
    }
}
