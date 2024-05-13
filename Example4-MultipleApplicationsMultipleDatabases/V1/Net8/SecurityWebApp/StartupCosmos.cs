using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.Cosmos;
using ServiceBricks.Security;
using ServiceBricks.Security.Cosmos;
using ServiceBricks.ServiceBus.Azure;
using System.Configuration;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupCosmos
    {
        public StartupCosmos(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksServiceBusAzure(Configuration);
            services.AddServiceBricksLoggingCosmos(Configuration);
            services.AddServiceBricksSecurityCosmos(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingCosmos();
            app.StartServiceBricksSecurityCosmos();
            app.StartCustomWebsite(webHostEnvironment);
            app.StartServiceBricksServiceBusAzure();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupCosmos>>();
            logger.LogInformation("Application Started");
        }
    }
}