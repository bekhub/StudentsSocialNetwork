using System.Net.Http;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestServices.Services;
using RestServices.Settings;

namespace RestServices
{
    public static class StartupSetup
    {
        public static void AddRestApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IObisApiService, ObisApiService>();
            services.AddScoped<HttpClient>();
            services.AddScoped<BaseHttpService>();
            services.Configure<ObisApiSettings>(configuration.GetSection(nameof(ObisApiSettings)));
            services.Configure<TimeTableApiSettings>(configuration.GetSection(nameof(TimeTableApiSettings)));
        }
    }
}
