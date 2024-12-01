using ServiceBricks;
using ServiceBricks.Cache.SqlServer;
using ServiceBricks.Logging.SqlServer;
using ServiceBricks.Notification.SqlServer;
using ServiceBricks.Security.SqlServer;
using WebApp.Extensions;
using WebApp.Model;

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
            //services.AddServiceBricksServiceBusAzureTopic(Configuration); // optional
            services.AddServiceBricksLoggingSqlServer(Configuration);
            services.AddServiceBricksCacheSqlServer(Configuration);
            services.AddServiceBricksNotificationSqlServer(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration); // optional
            services.AddServiceBricksSecuritySqlServer(Configuration);
            ModuleRegistry.Instance.Register(WebAppModule.Instance); // Add to module registry for automapper (See Mapping folder)
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupSqlServer>>();
            logger.LogInformation("Application Started");
        }
    }
}