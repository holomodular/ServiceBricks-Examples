﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace WebApp
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment webHostEnvironment,
            IMapper mapper)
        {
            _next = next;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            // Handle API requests using problem details
            if (context.Request.Path.HasValue &&
                context.Request.Path.Value.StartsWith(@"/api/", StringComparison.InvariantCultureIgnoreCase))
            {
                context.Response.ContentType = @"application/problem+json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                ProblemDetails details;
                if (_webHostEnvironment.IsDevelopment())
                    details = _mapper.Map<ProblemDetails>(exception);
                else
                {
                    details = new ProblemDetails()
                    {
                        Type = "https://httpstatuses.com/" + context.Response.StatusCode,
                        Title = "System Error",
                        Status = context.Response.StatusCode,
                        Detail = "General Server Error",
                    };
                }
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(details));
            }
            else
            {
                // Handle View requests using Error page
                context.Response.Redirect("/Error");
            }
        }
    }
}