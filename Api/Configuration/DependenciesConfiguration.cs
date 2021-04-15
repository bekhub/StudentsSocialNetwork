using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Core.Entities;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Api.Configuration
{
    public static class DependenciesConfiguration
    {
        public static void AddJwtIdentity(this IServiceCollection services, IConfigurationSection jwtConfiguration)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
                {
                    opts.User.RequireUniqueEmail = true;
                    opts.Password.RequireDigit = false;
                    opts.Password.RequireLowercase = false;
                    opts.Password.RequireNonAlphanumeric = false;
                    opts.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<SsnDbContext>()
                .AddDefaultTokenProviders();

            var signingKey = new SymmetricSecurityKey(
                Encoding.Default.GetBytes(jwtConfiguration["Secret"]));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var issuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
            var audience = jwtConfiguration[nameof(JwtConfiguration.Audience)];
            var validFor = TimeSpan.FromMinutes(jwtConfiguration[nameof(JwtConfiguration.ValidFor)].AsInt());
            var refreshTokenTtl = jwtConfiguration[nameof(JwtConfiguration.RefreshTokenTtl)].AsInt();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,

                ValidateAudience = true,
                ValidAudience = audience,

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingCredentials.Key,

                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = issuer;
                options.Audience = audience;
                options.ValidFor = validFor;
                options.SigningCredentials = signingCredentials;
                options.RefreshTokenTtl = refreshTokenTtl;
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(configureOptions =>
                {
                    configureOptions.ClaimsIssuer = issuer;
                    configureOptions.TokenValidationParameters = tokenValidationParameters;
                    configureOptions.SaveToken = true;
                    configureOptions.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            // This is to enable parsing the token from an authorization header in the format 'Token {token}'
                            var token = context.HttpContext.Request.Headers["Authorization"];
                            if (token.Count > 0 && token[0].StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
                            {
                                context.Token = token[0]["Token ".Length..].Trim();
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                                context.Response.Headers.Add("Token-Expired", "true");
                            
                            return Task.CompletedTask;
                        },
                    };
                });
        }

        public static void AddCorsPolicy(this IServiceCollection services, string corsPolicy)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(corsPolicy, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                });

                var scheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer",
                    },
                };

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    [scheme] = Array.Empty<string>(),
                });

                setup.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "SSNBackend API",
                    Version = "v1",
                });
                setup.CustomSchemaIds(x => x.FullName);
                setup.DocInclusionPredicate((_, _) => true);
                setup.EnableAnnotations();
            });
        }
    }
}