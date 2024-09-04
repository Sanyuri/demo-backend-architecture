using Microsoft.AspNetCore.Http;

namespace DemoBackendArchitecture.Application.Middlewares;

public class CheckSumMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        
    }
}