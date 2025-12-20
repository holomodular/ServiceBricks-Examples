using ServiceBricks;
using ServiceBricks.Cache;
using ServiceBricks.Logging.Cosmos;
using ServiceBricks.Notification.Cosmos;
using ServiceBricks.Security.Member;
using ServiceBricks.ServiceBus.Azure;
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
            services.AddServiceBricksServiceBusAzureQueue(Configuration); // Basic
            //services.AddServiceBricksServiceBusAzureTopic(Configuration); // Standard/Premium
            services.AddServiceBricksLoggingCosmos(Configuration);
            services.AddServiceBricksCacheClientForService(Configuration);
            services.AddServiceBricksNotificationCosmos(Configuration);
            services.AddServiceBricksSecurityMember(Configuration);
            services.AddServiceBricksComplete(Configuration);
            ProblemDetailsMappingProfile.Register(MapperRegistry.Instance);
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