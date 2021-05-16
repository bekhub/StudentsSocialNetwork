using Api.Endpoints.Posts;
using Api.Endpoints.Registration;
using Api.Endpoints.StudentAccount;
using Api.Helpers;
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
            services.AddScoped<RegistrationService>();
            services.AddScoped<StudentAccountService>();
            services.AddScoped<IFileSystem, CloudinaryFileSystem>();
            services.AddScoped<PostService>();
            services.AddTransient<IFireAndForgetHandler, FireAndForgetHandler>();
        }
    }
}
