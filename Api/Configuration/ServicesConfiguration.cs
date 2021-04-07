using Api.Services;
using Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<EmailService>();
            services.AddScoped<StudentsService>();
            services.AddScoped<IFileSystem, WebFileSystem>();
        }
    }
}
