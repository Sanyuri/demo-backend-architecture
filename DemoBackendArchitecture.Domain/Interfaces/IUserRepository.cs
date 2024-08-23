using DemoBackendArchitecture.Domain.Entities;

namespace DemoBackendArchitecture.Domain.Interfaces;

public interface IUserRepository : IGenericRepository<User>
{
    User? GetUserByEmail(string? userEmail);
    User Add(User user);
}