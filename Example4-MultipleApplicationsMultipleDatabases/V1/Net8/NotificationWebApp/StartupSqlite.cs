using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.Sqlite;
using ServiceBricks.Notification.Sqlite;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.Security.Member;
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
            services.AddServiceBricksServiceBusAzure(Configuration);
            services.AddServiceBricksLoggingSqlite(Configuration);
            services.AddServiceBricksNotificationSqlite(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);
            services.AddServiceBricksSecurityMember(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingSqlite();
            app.StartServiceBricksNotificationSqlite();
            app.StartServiceBricksSecurityMember();
            app.StartCustomWebsite(webHostEnvironment);
            app.StartServiceBricksServiceBusAzure();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupSqlite>>();
            logger.LogInformation("Application Started");
        }
    }
}