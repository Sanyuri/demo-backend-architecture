using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Infrastructure.Helpers;

public class AddRepositoryConfiguration
{
    public static void AddRepositories(IServiceCollection services, IConfiguration configuration)
    {
        
        
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Namespace.Contains("DemoBackendArchitecture.Infrastructure.Repositories") ||
                        c.Namespace.Contains("DemoBackendArchitecture.Domain.Interfaces"))
            .AsPublicImplementedInterfaces(ServiceLifetime.Scoped);
    }
}