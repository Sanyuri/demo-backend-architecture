using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using DemoBackendArchitecture.Application;
using DemoBackendArchitecture.Application.Common.Model.Product;
using DemoBackendArchitecture.Application.Common.Model.User;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Infrastructure;
using DemoBackendArchitecture.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.API.Configs;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        // services.AddDbContextPool<CoreContextPooling>((serviceProvider, dbContextBuilder) =>
        // {
        //     var connectionString = connectionStringPlaceHolder?
        //         .Replace("{dbName}",
        //             ConfigurationConstant.DefaultDatabase
        //         );
				    //
        //     dbContextBuilder.UseSqlServer(connectionString, options =>
        //     {
        //         options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        //     });
        // });
    }

    public static void ConfigureLayersServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplicationServices();
        services.AddRepositories();
    }

    public static void ConfigurePasswordHasher(this IServiceCollection services, IConfiguration configuration)
    {
        // Register PasswordHasher
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
    }

    public static void ConfigureAutoMapper(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Automapper
        services.AddAutoMapper(typeof(Product).Assembly, typeof(ProductDto).Assembly);
        services.AddAutoMapper(typeof(User).Assembly, typeof(UserSignInRequest).Assembly);
        services.AddAutoMapper(typeof(User).Assembly, typeof(UserSignInResponse).Assembly);
        services.AddAutoMapper(typeof(User).Assembly, typeof(UserSignUpRequest).Assembly);
        services.AddAutoMapper(typeof(User).Assembly, typeof(UserSignUpResponse).Assembly);
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
        // Add HttpContextAccessor
        services.AddHttpContextAccessor();
        // Add Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}