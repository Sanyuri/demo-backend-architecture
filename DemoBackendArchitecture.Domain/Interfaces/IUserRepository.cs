using DemoBackendArchitecture.Domain.Entities;

namespace DemoBackendArchitecture.Domain.Interfaces;

public interface IUserRepository : IUnitOfWork<User>
{
    Task<User> GetUserByEmailAsync(string email);
}