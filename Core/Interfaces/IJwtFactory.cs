using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IJwtFactory
    {
        /// <summary>
        /// Creates the jwt token.
        /// </summary>
        Task<(string token, DateTime expiration)> CreateTokenAsync(string userId, string email, IEnumerable<Claim> additionalClaims = default);
        
        /// <summary>
        /// Creates the refresh token.
        /// </summary>
        RefreshToken CreateRefreshToken(string ipAddress);
    }
}
