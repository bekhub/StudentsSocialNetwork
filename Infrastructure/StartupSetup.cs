using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.Repositories;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class StartupSetup
    {
        public static void AddDbContexts(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SsnDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IJwtFactory, JwtFactory>();
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IStudentRepository, StudentRepository>();
        }
    }
}
