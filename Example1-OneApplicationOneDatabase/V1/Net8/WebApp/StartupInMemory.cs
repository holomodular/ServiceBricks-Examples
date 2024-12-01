using ServiceBricks;
using ServiceBricks.Cache.InMemory;
using ServiceBricks.Logging.InMemory;
using ServiceBricks.Notification.InMemory;
using ServiceBricks.Security.InMemory;
using WebApp.Extensions;
using WebApp.Model;

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
            //services.AddServiceBricksServiceBusAzure(Configuration);  // optional
            services.AddServiceBricksLoggingInMemory(Configuration);
            services.AddServiceBricksCacheInMemory(Configuration);
            services.AddServiceBricksNotificationInMemory(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration); // optional
            services.AddServiceBricksSecurityInMemory(Configuration);
            ModuleRegistry.Instance.Register(WebAppModule.Instance); // Add to module registry for automapper (See Mapping folder)
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupInMemory>>();
            logger.LogInformation("Application Started");
        }
    }
}