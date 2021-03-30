using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }
        
        public string ProfilePictureUrl { get; set; }

        public string CurrentCity { get; set; }
        
        public DateTime? SignUpDate { get; set; }

        public ICollection<Student> Students { get; set; }
    }
}
