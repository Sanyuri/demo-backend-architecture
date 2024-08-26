using Microsoft.Extensions.DependencyInjection;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Infrastructure;

public static class RepositoryExtension
{
    public static void AddRepositories(this IServiceCollection services)
    {
        // Register the repositories with auto register DI
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Repository"))
            .AsPublicImplementedInterfaces();
    }
}