namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebsite(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddHttpClient();
            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews();
            services.AddCors();
            services.AddMvc();            

            return services;
        }
    }
}