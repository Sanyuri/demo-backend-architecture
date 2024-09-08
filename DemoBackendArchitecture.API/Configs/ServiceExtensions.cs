using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using DemoBackendArchitecture.API.Helpers;
using DemoBackendArchitecture.API.Mappings;
using DemoBackendArchitecture.Application.Configs;
using DemoBackendArchitecture.Application.Interfaces;
using DemoBackendArchitecture.Application.Mappings;
using DemoBackendArchitecture.Application.Services;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;
using DemoBackendArchitecture.Infrastructure.Helpers;
using DemoBackendArchitecture.Infrastructure.Repositories;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NetCore.AutoRegisterDi;
using Org.BouncyCastle.Tls;
using TenantInfo = DemoBackendArchitecture.Infrastructure.Helpers.MultiTenancy.TenantInfo;

namespace DemoBackendArchitecture.API.Configs;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        // services.AddDbContext<ApplicationDbContext>(options =>
        //     options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
        //     .AddSingleton<ApplicationDbContext>();

        // services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        // {
        //     var multiTenantContext = serviceProvider.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.GetMultiTenantContext<TenantInfo>();
        //
        //     var tenantInfo = multiTenantContext?.TenantInfo;
        //     
        //     options.UseSqlServer(tenantInfo?.ConnectionString ?? configuration.GetConnectionString("DefaultConnection"));
        // });
        
        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var httpContext = httpContextAccessor.HttpContext;
            
            var multiTenantContext = httpContext?.GetMultiTenantContext<TenantInfo>();
            var tenantInfo = multiTenantContext?.TenantInfo;
            
            
            options.UseSqlServer(tenantInfo?.ConnectionString ?? configuration.GetConnectionString("DefaultConnection"));
        });
    }

    public static void ConfigureLayersServices(this IServiceCollection services, IConfiguration configuration)
    {
    
        // // Register services for Application layer
        // services.AddScoped<IProductService, ProductService>();
        // services.AddScoped<IUserService, UserService>();
        // services.AddScoped<IRoleService, RoleService>();
        // // Register services for Infrastructure layer
        // services.AddScoped<IProductRepository, ProductRepository>();
        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IRoleRepository, RoleRepository>();
        // services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        //
        
        AddRepositoryConfiguration.AddRepositories(services, configuration);
        
        AddServiceConfiguration.AddServices(services, configuration);
        
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Namespace.Contains("DemoBackendArchitecture.API.Helpers"))
            .AsPublicImplementedInterfaces();
        

    }

    public static void ConfigurePasswordHasher(this IServiceCollection services, IConfiguration configuration)
    {
        // Register PasswordHasher
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }

    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Automapper
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly, typeof(ProductMapping).Assembly);
        services.AddAutoMapper(typeof(UserMappingProfile).Assembly, typeof(UserMapping).Assembly);
    }

    public static void ConfigureJwtBearer(this IServiceCollection services, IConfiguration configuration)
    {
        //Configure JwtBearer
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"] ?? string.Empty))
                };
            });
    }

    public static void ConfigureAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Authorization
        services.AddAuthorizationBuilder()
            // Configure Authorization
            .AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Role, "Admin"))
            .AddPolicy("User", policy => policy.RequireClaim(ClaimTypes.Role, "User"));
    }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure controllers and other services
        services.AddControllers()
            .AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve; });
        
        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        services.AddHangfire(options => options.UseSqlServerStorage(configuration.GetConnectionString("HangfireDatabaseConnection")));

        services.AddHangfireServer();
    }
}