using DemoBackendArchitecture.Domain.Entities;

namespace DemoBackendArchitecture.Application.Common.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProducts();
}