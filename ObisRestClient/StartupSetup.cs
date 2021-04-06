using System.Net.Http;
using Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ObisRestClient
{
    public static class StartupSetup
    {
        public static void AddObisApiServices(this IServiceCollection services, IConfigurationSection obisApiSettings)
        {
            services.AddScoped<IRestApiService, RestApiService>();
            services.AddScoped<HttpClient>();
            services.AddScoped<HttpService>();
            services.Configure<ObisApiSettings>(obisApiSettings);
        }
    }
}
