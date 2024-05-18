using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceBricks;
using System.Text.Json.Serialization;
using WebApp.Model;
using ServiceBricks.Security;

namespace WebApp.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomWebsite(this IServiceCollection services, IConfiguration Configuration)
        {
            // Add to module registry (just for automapper)
            ModuleRegistry.Instance.RegisterItem(typeof(WebAppModule), new WebAppModule());

            // THIS CLIENT WEBSITE DOES NOT RUN ANY SERVICEBRICKS MICROSERVICES
            // Add this for exception middleware
            services.AddHttpContextAccessor();
            services.AddOptions();
            services.Configure<ApiOptions>(Configuration.GetSection(ServiceBricks.ServiceBricksConstants.APPSETTING_APIOPTIONS));

            services.AddControllers();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddCors();
            services.AddMvc();

            // Add Swagger setup
            services.AddCustomSwagger(Configuration);

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddEndpointsApiExplorer();
            var apiVersioningBuilder = services.AddApiVersioning(options =>
            {
                options.ApiVersionReader = new MediaTypeApiVersionReader();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });
            apiVersioningBuilder.AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.ResolveConflictingActions(descriptions =>
                {
                    return descriptions.First();
                });
                options.CustomSchemaIds(x => x.FullName);
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "API v1", Version = "1.0" });
                options.SwaggerDoc("v2", new OpenApiInfo { Title = "API v2", Version = "2.0" });
                options.OperationFilter<SwaggerRemoveVersionOperationFilter>();
                options.DocumentFilter<SwaggerReplaceVersionDocumentFilter>();
                options.DocInclusionPredicate((docName, apiDesc) =>
                {
                    return docName == apiDesc.GroupName;
                });
            });
            return services;
        }
    }
}