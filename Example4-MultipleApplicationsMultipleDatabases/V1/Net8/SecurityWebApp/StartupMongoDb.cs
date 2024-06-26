using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.MongoDb;
using ServiceBricks.Security.MongoDb;
using ServiceBricks.ServiceBus.Azure;
using System.Configuration;
using WebApp.Extensions;

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
            services.AddServiceBricksServiceBusAzure(Configuration);
            services.AddServiceBricksLoggingMongoDb(Configuration);
            services.AddServiceBricksSecurityMongoDb(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingMongoDb();
            app.StartServiceBricksSecurityMongoDb();
            app.StartCustomWebsite(webHostEnvironment);
            app.StartServiceBricksServiceBusAzure();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupMongoDb>>();
            logger.LogInformation("Application Started");
        }
    }
}