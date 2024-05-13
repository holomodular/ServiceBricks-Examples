using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.MongoDb;
using ServiceBricks.Cache.MongoDb;
using ServiceBricks.Notification.MongoDb;
using ServiceBricks.Security.MongoDb;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.ServiceBus.Azure;

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
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingMongoDb();
            app.StartServiceBricksCacheMongoDb();
            app.StartServiceBricksNotificationMongoDb();
            app.StartServiceBricksSecurityMongoDb();
            app.StartCustomWebsite(webHostEnvironment);
            //app.StartServiceBricksServiceBusAzure();  // optional
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupMongoDb>>();
            logger.LogInformation("Application Started");
        }
    }
}