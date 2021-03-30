using Microsoft.AspNetCore.Builder;

namespace Api.Configuration
{
    public static class MiddlewareConfiguration
    {
        public static void UseSwaggerConfiguration(this IApplicationBuilder app, string endpointName)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.RoutePrefix = string.Empty;

                setup.SwaggerEndpoint(
                    url: "/swagger/v1/swagger.json",
                    name: endpointName);
            });
        }
    }
}
