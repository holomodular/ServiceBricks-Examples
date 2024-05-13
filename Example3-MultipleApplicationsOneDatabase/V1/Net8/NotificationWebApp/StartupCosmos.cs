using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.Cosmos;
using ServiceBricks.Notification.Cosmos;
using ServiceBricks.Security;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.Security.Member;
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
            services.AddServiceBricksServiceBusAzure(Configuration);
            services.AddServiceBricksLoggingCosmos(Configuration);
            services.AddServiceBricksNotificationCosmos(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);
            services.AddServiceBricksSecurityMember(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingCosmos();
            app.StartServiceBricksNotificationCosmos();
            app.StartServiceBricksSecurityMember();
            app.StartCustomWebsite(webHostEnvironment);
            app.StartServiceBricksServiceBusAzure();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupCosmos>>();
            logger.LogInformation("Application Started");
        }
    }
}