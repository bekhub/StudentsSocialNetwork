using System;
using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class ApplicationUser : IdentityUser, IAggregateRoot
    {
        public string Firstname { get; set; }
        
        public string Lastname { get; set; }
        
        public string ProfilePictureUrl { get; set; }

        public string CurrentCity { get; set; }
        
        public DateTime? SignUpDate { get; set; }

        public List<Student> Students { get; set; } = new();

        public List<RefreshToken> RefreshTokens { get; set; } = new();

        public bool OwnsToken(string token)
        {
            return RefreshTokens.Any(x => x.Token == token);
        }

        public RefreshToken GetRefreshToken(string token)
        {
            return RefreshTokens.SingleOrDefault(x => x.Token == token);
        }
        
        public void RemoveOldRefreshTokens(int refreshTokenTtl)
        {
            RefreshTokens.RemoveAll(x => 
                !x.IsActive && x.Created.AddDays(refreshTokenTtl) <= DateTime.UtcNow);
        }
    }
}
