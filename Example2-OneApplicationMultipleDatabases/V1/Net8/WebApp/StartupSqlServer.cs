using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.SqlServer;
using ServiceBricks.Cache.SqlServer;
using ServiceBricks.Notification.SqlServer;
using ServiceBricks.Security.SqlServer;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.ServiceBus.Azure;

namespace WebApp
{
    public class StartupSqlServer
    {
        public StartupSqlServer(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            //services.AddServiceBricksServiceBusAzure(Configuration);  // optional
            services.AddServiceBricksLoggingSqlServer(Configuration);
            services.AddServiceBricksCacheSqlServer(Configuration);
            services.AddServiceBricksNotificationSqlServer(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);  // optional
            services.AddServiceBricksSecuritySqlServer(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingSqlServer();
            app.StartServiceBricksCacheSqlServer();
            app.StartServiceBricksNotificationSqlServer();
            app.StartServiceBricksSecuritySqlServer();
            app.StartCustomWebsite(webHostEnvironment);
            //app.StartServiceBricksServiceBusAzure();  // optional
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupSqlServer>>();
            logger.LogInformation("Application Started");
        }
    }
}