using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.Application.Middlewares;

public class CsrfMiddleware(RequestDelegate next, IAntiforgery antiForgery)
{
    private readonly RequestDelegate _next = next;
    private readonly IAntiforgery _antiForgery = antiForgery;
    // Compare this snippet from DemoBackendArchitecture.Application/Middlewares/CsrfMiddleware.cs:
    public async Task InvokeAsync(HttpContext context)
    {
        // Check if the request is a state-changing HTTP method
        if (HttpMethods.IsPost(context.Request.Method) || 
            HttpMethods.IsPut(context.Request.Method) ||
            HttpMethods.IsDelete(context.Request.Method))
        {
            //Get the endpoint information
            var endpoint = context.GetEndpoint();
            // Check if the endpoint is annotated with [IgnoreAntiForgeryToken]
            if (endpoint?.Metadata?.GetMetadata<IgnoreAntiforgeryTokenAttribute>() != null)
            {
                // Continue to the next middleware
                await _next(context);
                return;
            }
            try
            {
                // Validate the CSRF token
                await _antiForgery.ValidateRequestAsync(context);
            }
            catch (AntiforgeryValidationException)
            {
                // If validation fails, respond with 400 Bad Request or other appropriate status
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("Invalid CSRF token.");
                return;
            }
        }
        // Continue to the next middleware
        await _next(context);
    }
}