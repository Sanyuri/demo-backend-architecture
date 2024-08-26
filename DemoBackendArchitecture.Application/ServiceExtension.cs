using DemoBackendArchitecture.Application.Common.Interfaces;
using DemoBackendArchitecture.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application;

public static class ServiceExtension
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        // Register the services with auto register DI
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
    }
}