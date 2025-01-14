using ServiceBricks;
using ServiceBricks.Cache.Postgres;
using ServiceBricks.Logging.Postgres;
using ServiceBricks.Notification.Postgres;
using ServiceBricks.Security.Postgres;
using WebApp.Extensions;
using WebApp.Model;

namespace WebApp
{
    public class StartupPostgres
    {
        public StartupPostgres(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public virtual IConfiguration Configuration { get; set; }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddServiceBricks(Configuration);
            //services.AddServiceBricksServiceBusAzure(Configuration);  // optional
            services.AddServiceBricksLoggingPostgres(Configuration);
            services.AddServiceBricksCachePostgres(Configuration);
            services.AddServiceBricksNotificationPostgres(Configuration);
            //services.AddServiceBricksNotificationSendGrid(Configuration); // optional
            services.AddServiceBricksSecurityPostgres(Configuration);
            ModuleRegistry.Instance.Register(WebAppModule.Instance); // Add to module registry for automapper (See Mapping folder)
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupPostgres>>();
            logger.LogInformation("Application Started");
        }
    }
}