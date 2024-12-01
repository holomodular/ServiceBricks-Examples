using ServiceBricks;
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
            services.AddServiceBricksLoggingClient(Configuration);
            services.AddServiceBricksCacheClient(Configuration);
            services.AddServiceBricksNotificationClient(Configuration);
            services.AddServiceBricksSecurityClient(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupClient>>();
            logger.LogInformation("Application Started");
        }
    }
}