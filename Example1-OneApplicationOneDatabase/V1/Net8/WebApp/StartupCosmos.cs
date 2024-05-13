using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.Cosmos;
using ServiceBricks.Cache.Cosmos;
using ServiceBricks.Notification.Cosmos;
using ServiceBricks.Security;
using ServiceBricks.Security.Cosmos;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.ServiceBus.Azure;

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
            //services.AddServiceBricksServiceBusAzure(Configuration);  // optional
            services.AddServiceBricksLoggingCosmos(Configuration);
            services.AddServiceBricksCacheCosmos(Configuration);
            services.AddServiceBricksNotificationCosmos(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);  // optional
            services.AddServiceBricksSecurityCosmos(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingCosmos();
            app.StartServiceBricksCacheCosmos();
            app.StartServiceBricksNotificationCosmos();
            app.StartServiceBricksSecurityCosmos();
            app.StartCustomWebsite(webHostEnvironment);
            //app.StartServiceBricksServiceBusAzure();  // optional
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupCosmos>>();
            logger.LogInformation("Application Started");
        }
    }
}