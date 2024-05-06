﻿using Microsoft.AspNetCore.CookiePolicy;
using ServiceBricks;
using ServiceBricks.Logging;
using ServiceBricks.Security;
using SQLitePCL;

namespace WebApp.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        private static IApplicationBuilder RegisterMiddleware(this IApplicationBuilder app)
        {
            // Microsoft.Extensions.Logging middleware
            app.UseMiddleware<CustomLoggerMiddleware>();

            // Web request logging middleware
            app.UseMiddleware<WebRequestMessageMiddleware>();

            // Exception handling middleware
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }

        public static IApplicationBuilder StartCustomWebsite(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (!env.IsDevelopment())
                app.UseHsts();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            // Register Middleware after UseAuth() so user context is available
            app.RegisterMiddleware();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                    x.SwaggerEndpoint("/swagger/v2/swagger.json", "API v2");
                });
            }

            return app;
        }
    }
}