using Hangfire;

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
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoint =>
        {
            endpoint.MapControllers();
            endpoint.MapHangfireDashboard();
        });
        
        //Add HangfireDashBoard
        app.UseHangfireDashboard();
    }
}