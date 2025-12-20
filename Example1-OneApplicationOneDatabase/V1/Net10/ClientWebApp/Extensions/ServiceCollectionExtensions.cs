namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebsite(this IServiceCollection services, IConfiguration Configuration)
        {
            //services.AddHttpContextAccessor();
            services.AddHttpClient(); // Needed for ApiClients
            
            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddCors();
            services.AddMvc();
            return services;
        }
    }
}