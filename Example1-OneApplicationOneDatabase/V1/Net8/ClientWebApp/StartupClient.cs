using ServiceBricks.Cache;
using ServiceBricks.Logging;
using ServiceBricks.Notification;
using ServiceBricks.Security;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupClient
    {
        public StartupClient(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            // Add ServiceBricks clients for each microservice
            services.AddServiceBricksLoggingClient(Configuration);
            services.AddServiceBricksCacheClient(Configuration);
            services.AddServiceBricksNotificationClient(Configuration);
            services.AddServiceBricksSecurityClient(Configuration);

            // Standard website
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            // Standard website
            app.StartCustomWebsite(webHostEnvironment);

            // Log a message website started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupClient>>();
            logger.LogInformation("Application Started");
        }
    }
}