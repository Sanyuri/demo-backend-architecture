using AutoMapper;
using DemoBackendArchitecture.Application.DTOs;
using DemoBackendArchitecture.Application.Interfaces;
using DemoBackendArchitecture.Domain.Entities;
using DemoBackendArchitecture.Domain.Interfaces;

namespace DemoBackendArchitecture.Application.Services;

public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
{
    public void CreateProduct(ProductDto productDto)
    {
        //Mapping the productDto to a Product entity
        var product = mapper.Map<Product>(productDto);
        //Setting the CreatedAt and UpdatedAt properties of the product
        product.CreatedAt = DateTime.UtcNow;
        product.UpdatedAt = DateTime.UtcNow;
        //Calling the Add method of the productRepository
        productRepository.AddAsync(product);
    }

    public ProductDto? GetProductById(int id)
    {
        //Calling the GetById method of the productRepository
        var product = productRepository.GetByIdAsync(id);
        //Returning the product if it is not null, otherwise returning null
        return product == null ? null : mapper.Map<ProductDto>(product);
    }
    
    public List<ProductDto> GetProductListByName(string name)
    {
        //Calling the GetAll method of the productRepository
        var products = productRepository.GetByName(name).ToList();
        //Mapping the list of products to a list of ProductDto
        return mapper.Map<List<ProductDto>>(products);
    }

    public void UpdateProduct(int id, ProductDto productDto)
    {
        //Calling the GetById method of the productRepository
        var product = productRepository.GetByIdAsync(id).Result;
        //Returning if the product is null
        if(product == null)
        {
            return;
        }
        //Mapping the properties of the productDto to the product
        mapper.Map(productDto, product);
        //Setting other properties of the product
        product.UpdatedAt = DateTime.UtcNow;
        //Calling the Update method of the productRepository
        productRepository.Update(product);
}

    public void DeleteProduct(int id)
    {
        productRepository.DeleteById(id);
    }
}