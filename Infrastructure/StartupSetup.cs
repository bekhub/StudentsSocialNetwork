using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class StartupSetup
    {
        public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString) =>
            services.AddDbContext<SsnDbContext>(options => options.UseNpgsql(connectionString));
    }
}
