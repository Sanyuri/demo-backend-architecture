using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;

namespace DemoBackendArchitecture.Infrastructure.Repositories;

public class RoleRepository(ApplicationDbContext context) : GenericRepository<Role>(context), IRoleRepository
{
    public Role? GetRoleByName(string roleName)
    {
        //Get the role by name from the database
        return context.Roles.FirstOrDefault(r => r.RoleName == roleName);
    }
}