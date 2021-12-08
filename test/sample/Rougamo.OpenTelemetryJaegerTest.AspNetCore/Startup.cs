using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Rougamo.OpenTelemetry;
using Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services;
using Rougamo.OpenTelemetryJaegerTest.AspNetCore.Services.Impl;

namespace Rougamo.OpenTelemetryJaegerTest.AspNetCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddHttpClient();

            services.AddOpenTelemetryTracing(builder =>
            {
                builder
                    .AddRougamoSource()
                    .AddAspNetCoreInstrumentation()
                    .AddJaegerExporter();
            });
            services.AddOpenTelemetryRougamo(options =>
            {
                options.ArgumentsStoreType = ArgumentsStoreType.Tag;
            });
            services.AddRougamoJsonSerializer();

            services
                .AddScoped<ITestService, TestService>()
                .AddScoped<IRemoteService, RemoteService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
