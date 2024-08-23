using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;
using DemoBackendArchitecture.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DemoBackendArchitecture.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : GenericRepository<User>(context), IUserRepository
{
    public User? GetUserByEmail(string? userEmail)
    {
        //Implementation to get user by email
        return context.Users.Include(r => r.Role).FirstOrDefault(u => u.Email == userEmail);
    }

    public User Add(User user)
    {
        //Implementation to add user
        context.Users.Add(user);
        context.SaveChanges();
        return user;
    }
}