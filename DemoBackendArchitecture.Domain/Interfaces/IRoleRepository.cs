using DemoBackendArchitecture.Domain.Entities;

namespace DemoBackendArchitecture.Domain.Interfaces;

public interface IRoleRepository : IGenericRepository<Role>
{
    Role? GetRoleByName(string roleName);
}