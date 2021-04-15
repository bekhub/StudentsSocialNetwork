using System;
using System.Threading.Tasks;
using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var context = services.GetRequiredService<SsnDbContext>();
                    await IdentityDbContextSeed.SeedAsync(userManager, roleManager, context);
                }
                catch (Exception e)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(e, "An error occurred seeding the DB.");
                }
            }
            
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddFile("logs/web-api-{Date}.txt");
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseSentry("https://abb483c51f784d56b34cbf8f6389e24b@o569806.ingest.sentry.io/5715991");
                });
    }
}
