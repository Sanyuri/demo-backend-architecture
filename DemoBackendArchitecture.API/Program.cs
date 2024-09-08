using DemoBackendArchitecture.API.Configs;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Strategies;
using TenantInfo = DemoBackendArchitecture.Infrastructure.Helpers.MultiTenancy.TenantInfo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddMultiTenant<TenantInfo>()
    .WithHeaderStrategy("X-Tenant-ID")
    .WithRouteStrategy("tenant")
    .WithConfigurationStore();
//Configure services
builder.Services.ConfigureServices(builder.Configuration);
builder.Services.ConfigureDbContext(builder.Configuration);
builder.Services.ConfigureLayersServices(builder.Configuration);
builder.Services.ConfigurePasswordHasher(builder.Configuration);
builder.Services.ConfigureAutoMapper(builder.Configuration);
builder.Services.ConfigureJwtBearer(builder.Configuration);
builder.Services.ConfigureAuthorization(builder.Configuration);
var app = builder.Build();

//Configure middleware
app.UseMultiTenant();
app.ConfigureMiddleware(app.Environment);

//Run the application
await app.RunAsync();