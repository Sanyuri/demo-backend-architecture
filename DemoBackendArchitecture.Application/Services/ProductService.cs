using DemoBackendArchitecture.Application.Common.Interfaces;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;

namespace DemoBackendArchitecture.Application.Services;

public class ProductService(IProductRepository productRepository) :IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _productRepository.GetAllAsync();
    }
}