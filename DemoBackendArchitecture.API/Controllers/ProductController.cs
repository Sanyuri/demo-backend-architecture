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
}