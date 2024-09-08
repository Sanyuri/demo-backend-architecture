using Finbuckle.MultiTenant.Abstractions;

namespace DemoBackendArchitecture.Infrastructure.Helpers.MultiTenancy;

public class TenantInfo : ITenantInfo
{
    public string? Id { get; set; }
    public string? Identifier { get; set; }
    public string? Name { get; set; }
    public string? ConnectionString { get; set; }
    public string? ChallengeScheme { get; set; }
}