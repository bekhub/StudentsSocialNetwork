using Api.Configuration;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure;

namespace Api
{
    public class Startup
    {
        private const string CORS_POLICY = "CorsPolicy";
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCorsPolicy(CORS_POLICY);
            services.AddDbContexts(Configuration.GetConnectionString("DefaultConnection"));
            services.AddControllers();
            services.AddSwagger();
        }

        public void ConfigureContainerBuilder(ContainerBuilder builder)
        {
            builder.RegisterModule(new DefaultInfrastructureModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors(CORS_POLICY);

            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseSwaggerConfiguration("SSNBackend v1");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
