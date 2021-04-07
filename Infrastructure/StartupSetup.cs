using Core.Interfaces;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Infrastructure.Configuration;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class StartupSetup
    {
        public static void AddDbContexts(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SsnDbContext>(options => options.UseNpgsql(connectionString));
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICurrentUserAccessor, CurrentUserAccessor>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<IEncryptionService, EncryptionService>(_ => new EncryptionService(configuration["EncryptionKey"]));
            services.Configure<EmailSettings>(configuration.GetSection(nameof(EmailSettings)));
        }
    }
}
