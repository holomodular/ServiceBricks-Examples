using Microsoft.AspNetCore.Hosting;
using ServiceBricks;
using ServiceBricks.Logging.AzureDataTables;
using System.Configuration;
using WebApp.Extensions;
using ServiceBricks.Security.Member;
using ServiceBricks.ServiceBus.Azure;

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
            services.AddServiceBricksServiceBusAzure(Configuration);
            services.AddServiceBricksLoggingAzureDataTables(Configuration);
            services.AddServiceBricksSecurityMember(Configuration);
            services.AddCustomWebsite(Configuration);
            services.AddServiceBricksComplete();
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment webHostEnvironment)
        {
            app.StartServiceBricks();
            app.StartServiceBricksLoggingAzureDataTables();
            app.StartServiceBricksSecurityMember();
            app.StartCustomWebsite(webHostEnvironment);
            app.StartServiceBricksServiceBusAzure();
            var logger = app.ApplicationServices.GetRequiredService<ILogger<StartupAzureDataTables>>();
            logger.LogInformation("Application Started");
        }
    }
}