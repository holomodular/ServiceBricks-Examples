using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.AzureDataTables;
using ServiceBricks.Notification.AzureDataTables;
using ServiceBricks.Security.Member;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.Security;

namespace WebApp
{
    public class StartupAzureDataTables
    {
        public StartupAzureDataTables(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksLoggingAzureDataTables(Configuration);
            services.AddServiceBricksNotificationAzureDataTables(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);
            services.AddServiceBricksSecurityMember(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingAzureDataTables();
            app.StartServiceBricksNotificationAzureDataTables();
            app.StartServiceBrickSecurityMember();
            app.StartCustomWebsite(webHostEnvironment);
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupAzureDataTables>>();
            logger.LogInformation("Application Started");
        }
    }
}