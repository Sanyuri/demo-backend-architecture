using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application.Configs;

public class AddServiceConfiguration
{
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Namespace.Contains("DemoBackendArchitecture.Application.Services"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
    }
}