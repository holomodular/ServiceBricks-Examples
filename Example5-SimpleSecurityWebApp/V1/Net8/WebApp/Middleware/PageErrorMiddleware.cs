using AutoMapper;
using Microsoft.Extensions.Options;
using ServiceBricks;

namespace WebApp.Middleware
{
    public class PageErrorMiddleware : TrapExceptionResponseMiddleware
    {
        public PageErrorMiddleware(RequestDelegate next, ILoggerFactory loggerFactory, IMapper mapper, IOptions<ApiOptions> apiOptions)
            : base(next, loggerFactory, mapper, apiOptions)
        {
        }

        public override async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (!httpContext.Request.Path.HasValue || !httpContext.Request.Path.Value.StartsWith("/api/", StringComparison.InvariantCultureIgnoreCase))
                {
                    _logger.LogError(ex, ex.Message);
                    httpContext.Response.Redirect("/error");
                }
                else
                    throw;
            }
        }
    }
}