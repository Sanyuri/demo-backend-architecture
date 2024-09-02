using DemoBackendArchitecture.API.Configs;
using Microsoft.AspNetCore.Antiforgery;

var builder = WebApplication.CreateBuilder(args);
//Configure services
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureLayersServices(builder.Configuration);
builder.Services.ConfigurePasswordHasher(builder.Configuration);
builder.Services.ConfigureAutoMapper(builder.Configuration);
builder.Services.ConfigureJwtBearer(builder.Configuration);
builder.Services.ConfigureAuthorization(builder.Configuration);
builder.Services.ConfigureCsrf();

var app = builder.Build();

//Configure middleware


var antiForgery = app.Services.GetRequiredService<IAntiforgery>();

app.Use((context, next) =>
{
    var requestPath = context.Request.Path.Value;

    if (string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)
        || string.Equals(requestPath, "/index.html", StringComparison.OrdinalIgnoreCase))
    {
        var tokenSet = antiForgery.GetAndStoreTokens(context);
        context.Response.Cookies.Append("XSRF-TOKEN", tokenSet.RequestToken!,
            new CookieOptions { HttpOnly = false });
    }

    return next(context);
});
app.ConfigureMiddleware(app.Environment);
app.ConfigureCsrfMiddleware();
//Run the application
await app.RunAsync();