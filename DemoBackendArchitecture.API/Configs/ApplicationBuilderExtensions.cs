using DemoBackendArchitecture.Application.Middlewares;

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
        app.UseMiddleware<CsrfMiddleware>();
        app.UseMiddleware<JwtMiddleware>();
        app.UseEndpoints(endpoint => 
            endpoint.MapControllers());
    }
}