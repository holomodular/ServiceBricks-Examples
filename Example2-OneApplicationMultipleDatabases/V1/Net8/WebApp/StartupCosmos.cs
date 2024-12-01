using ServiceBricks;
using ServiceBricks.Cache.Cosmos;
using ServiceBricks.Logging.Cosmos;
using ServiceBricks.Notification.Cosmos;
using ServiceBricks.Security.Cosmos;
using WebApp.Extensions;
using WebApp.Model;

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
            ModuleRegistry.Instance.Register(WebAppModule.Instance); // Add to module registry for automapper (See Mapping folder)
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupCosmos>>();
            logger.LogInformation("Application Started");
        }
    }
}