using ServiceBricks;
using ServiceBricks.Cache.MongoDb;
using ServiceBricks.Logging.MongoDb;
using ServiceBricks.Notification.MongoDb;
using ServiceBricks.Security.MongoDb;
using WebApp.Extensions;
using WebApp.Model;

namespace WebApp
{
    public class StartupMongoDb
    {
        public StartupMongoDb(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            //services.AddServiceBricksServiceBusAzure(Configuration);  // optional
            services.AddServiceBricksLoggingMongoDb(Configuration);
            services.AddServiceBricksCacheMongoDb(Configuration);
            services.AddServiceBricksNotificationMongoDb(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration); // optional
            services.AddServiceBricksSecurityMongoDb(Configuration);
            ModuleRegistry.Instance.Register(WebAppModule.Instance); // Add to module registry for automapper (See Mapping folder)
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupMongoDb>>();
            logger.LogInformation("Application Started");
        }
    }
}