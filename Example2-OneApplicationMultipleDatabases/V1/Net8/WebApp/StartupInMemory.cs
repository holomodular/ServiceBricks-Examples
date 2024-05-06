using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Cache.InMemory;
using ServiceBricks.Logging.InMemory;
using ServiceBricks.Notification.InMemory;
using ServiceBricks.Notification.SendGrid;
using ServiceBricks.Security.InMemory;
using System.Configuration;
using WebApp.Extensions;

namespace WebApp
{
    public class StartupInMemory
    {
        public StartupInMemory(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            services.AddServiceBricksLoggingInMemory(Configuration);
            services.AddServiceBricksCacheInMemory(Configuration);
            services.AddServiceBricksNotificationInMemory(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration);
            services.AddServiceBricksSecurityInMemory(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingInMemory();
            app.StartServiceBricksCacheInMemory();
            app.StartServiceBricksNotificationInMemory();
            app.StartServiceBricksSecurityInMemory();
            app.StartCustomWebsite(webHostEnvironment);
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupInMemory>>();
            logger.LogInformation("Application Started");
        }
    }
}