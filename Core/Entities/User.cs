using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User : IdentityUser
    {
        public string ProfilePictureUrl { get; set; }

        public bool IsActive { get; set; }

        public string CurrentCity { get; set; }
        
        public DateTime SignUpDate { get; set; }

        public DateTime LastSeen { get; set; }

        public IEnumerable<Student> Student { get; set; }
    }
}
