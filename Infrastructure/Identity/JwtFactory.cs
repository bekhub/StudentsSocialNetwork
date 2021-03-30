﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace Infrastructure.Identity
{
    public class JwtFactory : IJwtFactory
    {
        private readonly JwtConfiguration _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;

        public JwtFactory(IOptions<JwtConfiguration> jwtOptions, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions.Value;
            ThrowIfInvalidOptions(_jwtOptions);
        }

        public async Task<string> CreateTokenAsync(string userId, string email, IEnumerable<Claim> additionalClaims)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var roles = await _userManager.GetRolesAsync(user);
            
            var claims = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, userId),
                    new(JwtRegisteredClaimNames.Email, email),
                    new(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                    new(JwtRegisteredClaimNames.Iat, new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(), 
                        ClaimValueTypes.Integer64),
                };
            claims.AddRange(additionalClaims);
            claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
            
            _jwtOptions.ValidFor = TimeSpan.FromMinutes(60);

            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                notBefore: _jwtOptions.NotBefore,
                expires: _jwtOptions.Expiration,
                signingCredentials: _jwtOptions.SigningCredentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            
            return tokenHandler.WriteToken(jwt);
        }

        public Task<string> CreateTokenAsync(string userId, string email)
        {
            return CreateTokenAsync(userId, email, new List<Claim>());
        }

        private static void ThrowIfInvalidOptions(JwtConfiguration options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.");
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentException(nameof(JwtConfiguration.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentException(nameof(JwtConfiguration.JtiGenerator));
            }
        }
    }
}