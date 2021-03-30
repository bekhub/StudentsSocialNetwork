using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IJwtFactory
    {
        /// <summary>
        /// Creates the jwt token.
        /// </summary>
        Task<string> CreateTokenAsync(string userId, string email, IEnumerable<Claim> additionalClaims);
        
        /// <summary>
        /// Creates the jwt token.
        /// </summary>
        Task<string> CreateTokenAsync(string userId, string email);
    }
}
