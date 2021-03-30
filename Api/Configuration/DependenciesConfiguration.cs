using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
                .AddEntityFrameworkStores<SsnDbContext>();

            var signingKey = new SymmetricSecurityKey(
                Encoding.Default.GetBytes(jwtConfiguration["Secret"]));

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtConfiguration[nameof(JwtConfiguration.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
            };

            services.Configure<JwtConfiguration>(options =>
            {
                options.Issuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
                options.Audience = jwtConfiguration[nameof(JwtConfiguration.Audience)];
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtConfiguration[nameof(JwtConfiguration.Issuer)];
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
                            context.Token = token[0].Substring("Token ".Length).Trim();
                        }

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
