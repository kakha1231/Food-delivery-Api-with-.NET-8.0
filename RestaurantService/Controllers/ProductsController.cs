using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantService.Dtos.Request;
using RestaurantService.Dtos.Response;
using RestaurantService.Services;

namespace RestaurantService.Controllers;

[ApiController]
public class ProductsController : ControllerBase
{
    private readonly ProductManagementService _productManagementService;
    
    public ProductsController(ProductManagementService productManagementService)
    {
        _productManagementService = productManagementService;
    }
    
    [HttpGet("/products")]

    public async Task<ActionResult<List<ProductResponseDto>>> GetProducts()
    {
        return Ok(await _productManagementService.GetProducts());
    }

    [HttpGet("/product/{productId}")]
    public async Task<ActionResult<ProductResponseDto>> GetProduct(int productId)
    {
        return Ok(await _productManagementService.GetProduct(productId));
    }

    [Authorize]
    [HttpPost("/add-Product")]
    public async Task<ActionResult<ProductResponseDto>> AddProduct(AddProductDto productDto)
    {
        var userId = User.Claims.First(u => u.Type == "Id").Value;
        return Ok(await _productManagementService.AddProduct(productDto, userId));
    }

    [Authorize]
    [HttpPut("/edit-product/{productId}")]
    public async Task<ActionResult<ProductResponseDto>> EditProduct(int productId, AddProductDto editProductDto)
    {
        var userId = User.Claims.First(u => u.Type == "Id").Value;
        return Ok(await _productManagementService.EditProduct(productId, editProductDto, userId));
    }
    
    
    
    
    
    
    
}