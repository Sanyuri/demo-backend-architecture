﻿using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using DemoBackendArchitecture.Application;
using DemoBackendArchitecture.Application.Common.Model.Email;
using DemoBackendArchitecture.Application.Common.Model.Jwt;
using DemoBackendArchitecture.Application.Common.Model.Product;
using DemoBackendArchitecture.Application.Common.Model.User;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;
using DemoBackendArchitecture.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DemoBackendArchitecture.API.Configs;

public static class ServiceExtensions
{
    public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
        // services.AddDbContextPool<ApplicationDbContext>((serviceProvider, dbContextBuilder) =>
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
        // Add EmailService
        services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
        //Call the AddApplication method from ConfigurationExtend
        ConfigurationExtend.ConfigureExtend(services);
        // Register Repositories
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
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
        var jwtSettings = configuration.GetSection("Jwt").Get<JwtSetting>();
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
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.Zero
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
    
    // configure middleware csrf
    public static void ConfigureCsrf(this IServiceCollection services)
    {
        services.AddAntiforgery(options =>
        {
            options.HeaderName = "X-CSRF-TOKEN";
            options.Cookie.Name = "CSRF-TOKEN";
            options.SuppressXFrameOptionsHeader = false;
            options.Cookie.HttpOnly = false;
        });
    }
    
    /// <summary>
    /// Configure cors
    /// </summary>
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy",
                builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials());
        });
    }
}