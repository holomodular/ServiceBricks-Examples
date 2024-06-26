using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.Sqlite;
using ServiceBricks.Cache.Sqlite;
using ServiceBricks.Notification.Sqlite;
using ServiceBricks.Security.Sqlite;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.ServiceBus.Azure;

namespace WebApp
{
    public class StartupSqlite
    {
        public StartupSqlite(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            //services.AddServiceBricksServiceBusAzure(Configuration);  // optional
            services.AddServiceBricksLoggingSqlite(Configuration);
            services.AddServiceBricksCacheSqlite(Configuration);
            services.AddServiceBricksNotificationSqlite(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration); // optional
            services.AddServiceBricksSecuritySqlite(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingSqlite();
            app.StartServiceBricksCacheSqlite();
            app.StartServiceBricksNotificationSqlite();
            app.StartServiceBricksSecuritySqlite();
            app.StartCustomWebsite(webHostEnvironment);
            //app.StartServiceBricksServiceBusAzure();  // optional
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupSqlite>>();
            logger.LogInformation("Application Started");
        }
    }
}