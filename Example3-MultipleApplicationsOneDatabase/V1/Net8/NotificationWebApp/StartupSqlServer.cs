using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.SqlServer;
using ServiceBricks.Notification.SqlServer;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.Security.Member;
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
            services.AddServiceBricksServiceBusAzure(Configuration);
            services.AddServiceBricksLoggingSqlServer(Configuration);
            services.AddServiceBricksNotificationSqlServer(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);
            services.AddServiceBricksSecurityMember(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingSqlServer();
            app.StartServiceBricksNotificationSqlServer();
            app.StartServiceBricksSecurityMember();
            app.StartCustomWebsite(webHostEnvironment);
            app.StartServiceBricksServiceBusAzure();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupSqlServer>>();
            logger.LogInformation("Application Started");
        }
    }
}