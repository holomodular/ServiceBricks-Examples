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

            // This section is dependent on the SeviceBricks:Api:ReturnResponseObject config.
            // If true, errors will be returned in a response object.
            // If false, errors will be returned with problemdetails.
            services.AddControllers().ConfigureApiBehaviorOptions(setup =>
            {
                setup.InvalidModelStateResponseFactory = context =>
                {
                    if (context.HttpContext != null &&
                    context.HttpContext.Request != null &&
                    context.HttpContext.Request.Path.HasValue &&
                    context.HttpContext.Request.Path.Value.StartsWith(@"/api/", StringComparison.InvariantCultureIgnoreCase))
                    {
                        var apiOptions = context.HttpContext.RequestServices.GetRequiredService<IOptions<ApiOptions>>().Value;

                        if (apiOptions.ReturnResponseObject)
                        {
                            Response response = new Response();
                            foreach (var key in context.ModelState.Keys)
                            {
                                foreach (var err in context.ModelState[key].Errors)
                                {
                                    if (!string.IsNullOrEmpty(key))
                                        response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage, key));
                                    else
                                        response.AddMessage(ResponseMessage.CreateError(err.ErrorMessage));
                                }
                            }

                            var objectResult = new ObjectResult(response) { StatusCode = StatusCodes.Status400BadRequest };
                            return objectResult;
                        }
                        else
                        {
                            var vpd = new ValidationProblemDetails(context.ModelState);
                            var objectResult = new ObjectResult(vpd) { StatusCode = StatusCodes.Status400BadRequest };
                            return objectResult;
                        }
                    }
                    else
                    {
                        var vpd = new ValidationProblemDetails(context.ModelState);
                        var objectResult = new ObjectResult(vpd) { StatusCode = StatusCodes.Status400BadRequest };
                        return objectResult;
                    }
                };
            });
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddOptions();
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