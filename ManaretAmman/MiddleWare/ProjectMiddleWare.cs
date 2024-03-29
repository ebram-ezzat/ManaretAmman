﻿using Microsoft.AspNetCore.Http;
using System.Reflection.Metadata;

namespace ManaretAmman.MiddleWare
{
    public class ProjectMiddleware
    {
        private readonly RequestDelegate _next;

        public ProjectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            //string userAgent = context.Request.Headers["User-Agent"].ToString();
            //if (userAgent.Contains("Swagger"))
            //{
            //    await Console.Out.WriteLineAsync(userAgent);
            //}
            //api/Auth/Login
            
            if ((context.Request.Path == "/api/Configration/GetProjectUrl" && context.Request.Method == "GET") || (context.Request.Path.HasValue && context.Request.Path.Value.ToLower().Contains("swagger")))
            {
                await _next(context);
                return;
            }
            #region check by filter attribute on API 
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var hasMyCustomAttribute = endpoint.Metadata.Any(metadata => metadata is SkipHeaderFilterAttribute);

                if (hasMyCustomAttribute)
                {
                    await _next(context);
                    return;
                }
            }
            #endregion
            if (context.Request.Headers.TryGetValue("projectid", out var ProjectIdValue))
            {
                context.Items["ProjectId"] = ProjectIdValue.ToString();
            }
            else
            {
                // Handle the case when projectId is not provided
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("ProjectId is missing from the request header.");
                return;
            }

            await _next(context);
        }
    }
}