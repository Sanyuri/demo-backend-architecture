using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using NetCore.AutoRegisterDi;

namespace DemoBackendArchitecture.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : UnitOfWork<User>(context), IUserRepository
{
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await context.Users.Include(x => x.Role).FirstOrDefaultAsync(x => x.Email == email);
    }
}