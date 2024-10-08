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
builder.Services.ConfigureCors();

var app = builder.Build();

//Configure middleware
app.ConfigureMiddleware(app.Environment);
app.ConfigureCsrfMiddleware();
//Run the application
await app.RunAsync();