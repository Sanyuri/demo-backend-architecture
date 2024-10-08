using DemoBackendArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace DemoBackendArchitecture.Application.Middlewares;

public class JwtMiddleware(RequestDelegate next, ITokenService tokenService)
{
    private readonly RequestDelegate _next = next;
    private readonly ITokenService _tokenService = tokenService;
    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (token != null)
        {
            try
            {
                var principal = _tokenService.ValidateToken(token);
                context.User = principal;
            }catch
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Invalid token");
                return;
            }
        }
        
        await _next(context);
    }
}