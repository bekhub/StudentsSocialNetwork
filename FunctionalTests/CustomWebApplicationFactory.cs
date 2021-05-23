using System;
using System.Linq;
using Api;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UnitTests;

namespace FunctionalTests
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var host = builder.Build();


            // Get service provider.
            var serviceProvider = host.Services;

            // Create a scope to obtain a reference to the database
            // context (AppDbContext).
            using (var scope = serviceProvider.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<SsnDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();

                // Ensure the database is created.
                db.Database.EnsureCreated();

                try
                {
                    // Seed the database with test data.
                    IdentityDbContextSeed.SeedAsync(userManager, roleManager, db);
                    SeedData.PopulateTestData(db);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the database with test messages. Error: {Message}", ex.Message);
                }
            }

            host.Start();
            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseSolutionRelativeContentRoot("Api/")
                .ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType ==
                            typeof(DbContextOptions<SsnDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }
                    services.AddDbContext<SsnDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting");
                    });
                    services.AddScoped<IMediator, NoOpMediator>();
                });
        }
    }
}
