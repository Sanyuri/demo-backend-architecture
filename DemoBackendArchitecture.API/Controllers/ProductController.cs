using DemoBackendArchitecture.Application.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DemoBackendArchitecture.API.Controllers;

public class ProductController(IProductService productService) : BaseController
{
    private readonly IProductService _productService = productService;
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        // Get all products
        return Ok(await _productService.GetAllProducts());
    }

    // [HttpGet("{id}")]
    // public async Task<IActionResult> Get(int id)
    // {
    //     // Get product by id
    //     return Ok(await _productService.GetProductById(id));
    // }
    //
    // [HttpPost]
    // [AutoValidateAntiforgeryToken]
    // public async Task<IActionResult> Post([FromBody] ProductDto productDto)
    // {
    //     // Create new product
    //     return Ok(await _productService.CreateProduct(productDto));
    // }
}