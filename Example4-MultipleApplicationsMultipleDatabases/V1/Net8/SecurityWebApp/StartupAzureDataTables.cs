using ServiceBricks;
using ServiceBricks.Logging.AzureDataTables;
using ServiceBricks.Security.AzureDataTables;
using ServiceBricks.ServiceBus.Azure;
using WebApp.Extensions;
using WebApp.Model;

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
            services.AddServiceBricksServiceBusAzureTopic(Configuration);
            services.AddServiceBricksLoggingAzureDataTables(Configuration);
            services.AddServiceBricksSecurityAzureDataTables(Configuration);
            ModuleRegistry.Instance.Register(WebAppModule.Instance); // Add to module registry for automapper (See Mapping folder)
            services.AddServiceBricksComplete(Configuration);
            services.AddCustomWebsite(Configuration);
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();

            app.StartCustomWebsite(webHostEnvironment);

            // Log a message the website is started
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupAzureDataTables>>();
            logger.LogInformation("Application Started");
        }
    }
}