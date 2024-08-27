using AutoMapper;
using DemoBackendArchitecture.API.Models;
using DemoBackendArchitecture.Application.DTOs;
using DemoBackendArchitecture.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.API.Controllers;

[Authorize(Policy = "User")]
[Route("api/[controller]")]
[ApiController]
public class ProductController(IProductService productService, IMapper mapper) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult CreateProduct([FromBody] CreateProductRequest request)
    {
        //Mapping CreateProductRequest to ProductDto
        var productDto = mapper.Map<ProductDto>(request);
        //Calling the CreateProduct method of the productService
        productService.CreateProduct(productDto);
        return Ok("Product created successfully");
    }
    
    [HttpGet("{id:int}")]
    [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
    public IActionResult GetProductById(int id)
    {
        //Calling the GetProductById method of the productService
        var product = productService.GetProductById(id);
        //Returning the product if it is not null, otherwise returning NotFound
        
        return product == null? NotFound() : Ok(product);
    }
    
    [HttpPut("{id:int}")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        //Mapping UpdateProductRequest to ProductDto
        var productDto = mapper.Map<ProductDto>(request);
        //Setting the id of the productDto to the id of the product
        productDto.Id = id;
        //Calling the UpdateProduct method of the productService
        productService.UpdateProduct(id, productDto);
        return Ok("Product updated successfully");
    }
    
    [HttpDelete("{id:int}")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public IActionResult DeleteProduct(int id)
    {
        //Calling the DeleteProduct method of the productService
        productService.DeleteProduct(id);
        return Ok("Product deleted successfully");
    }
}