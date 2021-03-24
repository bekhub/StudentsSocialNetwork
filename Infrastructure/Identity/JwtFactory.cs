using System.Collections.Generic;
using System.Security.Claims;
using Core.Interfaces;

namespace Infrastructure.Identity
{
    public class JwtFactory : IJwtFactory
    {
        public string GenerateEncodedToken(string userId, string email, IEnumerable<Claim> additionalClaims)
        {
            throw new System.NotImplementedException();
        }
    }
}
