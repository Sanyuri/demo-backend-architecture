using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.Application.Middlewares;

public class CsrfMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAntiforgery _antiForgery;

    public CsrfMiddleware(RequestDelegate next, IAntiforgery antiForgery)
    {
        _next = next;
        _antiForgery = antiForgery;
    }
    
    // Compare this snippet from DemoBackendArchitecture.Application/Middlewares/CsrfMiddleware.cs:
    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var ignoreAntiforgery = endpoint.Metadata.GetMetadata<IgnoreAntiforgeryTokenAttribute>();
            if (ignoreAntiforgery != null)
            {
                await _next(context);
                return;
            }
        }
        
        if (string.Equals(context.Request.Method, "GET", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(context.Request.Method, "HEAD", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(context.Request.Method, "OPTIONS", StringComparison.OrdinalIgnoreCase) ||
            string.Equals(context.Request.Method, "TRACE", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }
        //Get token from cookie
        var tokens = context.Request.Cookies["XSRF-TOKEN"];
        //Get token from antiforgery service and validate it with the request token from the header
        if (context.Request.Headers.TryGetValue("X-CSRF-TOKEN", out var token) &&
            tokens == token)
        {
            await _next(context);
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync("CSRF token validation failed.");
        }
    }
}