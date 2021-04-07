using System.IO;
using Api.Configuration;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Infrastructure;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using ObisRestClient;

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
            services.AddInfrastructureServices(Configuration);
            services.AddObisApiServices(Configuration.GetSection("ObisApiSettings"));
            services.AddJwtIdentity(Configuration.GetSection(nameof(JwtConfiguration)));
            services.AddControllersWithViews();
            services.AddAutoMapper(typeof(Startup).Assembly);
            services.AddSwagger();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddServices();
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

            const string cacheMaxAge = "604800";
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "wwwroot")),
                RequestPath = "/static",
                OnPrepareResponse = ctx =>
                    ctx.Context.Response.Headers.Append(
                        "Cache-Control", $"public, max-age={cacheMaxAge}"),
            });

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
