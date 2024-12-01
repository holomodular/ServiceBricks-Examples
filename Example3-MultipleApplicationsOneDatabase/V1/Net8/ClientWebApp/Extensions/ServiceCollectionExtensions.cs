using Asp.Versioning;
using ServiceBricks;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebsite(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddOptions();
            services.Configure<ClientApiOptions>(Configuration.GetSection(ServiceBricksConstants.APPSETTING_CLIENT_APIOPTIONS));
            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddCors();
            services.AddMvc();

            services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            return services;
        }
    }
}