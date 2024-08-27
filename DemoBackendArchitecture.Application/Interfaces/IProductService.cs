using DemoBackendArchitecture.Application.DTOs;

namespace DemoBackendArchitecture.Application.Interfaces;


public interface IProductService
{
    void CreateProduct(ProductDto productDto);
    ProductDto? GetProductById(int id);
    void UpdateProduct(int id, ProductDto productDto);
    void DeleteProduct(int id);
}