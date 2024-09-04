using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Application;

public static class ConfigurationExtend
{
    public static void ConfigureExtend(IServiceCollection service)
    {
        service.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
    }
}