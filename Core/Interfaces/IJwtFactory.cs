using System.Collections.Generic;
using System.Security.Claims;

namespace Core.Interfaces
{
    public interface IJwtFactory
    {
        string GenerateEncodedToken(string userId, string email, IEnumerable<Claim> additionalClaims);
    }
}
