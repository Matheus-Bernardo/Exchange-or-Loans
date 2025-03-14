using ExchangeOrLoans.Dtos;
using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;
using ExchangeOrLoans.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductController: ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Product>> CreateProduct([FromForm] ProductCreateDto productDto)
    {
        return await _productService.CreateProduct(productDto);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        return await _productService.GetProducts();
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProductsById(int id)
    {
        return await _productService.GetProductById(id);
    }

    [Authorize]
    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<Product>> UpdateProduct([FromForm] ProductCreateDto productDto, [FromRoute] int id)
    {
        var updatedProduct = await _productService.UpdateProduct(productDto,id);
        if (updatedProduct == null) return  NotFound("product not found");
        
        return updatedProduct;
    }
    
}