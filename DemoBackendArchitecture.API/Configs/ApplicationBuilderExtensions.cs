using DemoBackendArchitecture.Application.Middlewares;
using Microsoft.AspNetCore.Antiforgery;

namespace DemoBackendArchitecture.API.Configs;

public static class ApplicationBuilderExtensions
{
    public static void ConfigureMiddleware(this IApplicationBuilder app, IHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        //Register middleware
        app.UseMiddleware<JwtMiddleware>();
        app.UseMiddleware<CsrfMiddleware>();
        app.UseEndpoints(endpoint => 
            endpoint.MapControllers());
    }
    
    public static void ConfigureCsrfMiddleware(this IApplicationBuilder app)
    {
        //Get required service from IAntiForgery
        app.Use(next => context =>
        {
            var tokens = app.ApplicationServices.GetRequiredService<IAntiforgery>();
            var request = context.Request.Path.Value;
            if (string.Equals(request, "/", StringComparison.OrdinalIgnoreCase)
                || string.Equals(request, "/index.html", StringComparison.OrdinalIgnoreCase))
            {
                var tokenSet = tokens.GetAndStoreTokens(context);
                context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
                    new CookieOptions { HttpOnly = false });
            }
            return next(context);
        });
    }
}