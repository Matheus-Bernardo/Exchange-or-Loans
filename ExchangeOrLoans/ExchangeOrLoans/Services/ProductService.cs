using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Utils;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public class ProductService : IProductService
{
    private readonly IImageService _imageService;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository, IUserRepository userRepository ,IImageService imageService)
    {
        _imageService = imageService;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }
    
    
    public async Task<ActionResult<Product>> CreateProduct(Product product , IFormFile image)
    {
        if (product == null) 
            return new BadRequestObjectResult("Product cannot be null");
        
        if(await _userRepository.GetUserById(product.IdUserSeller) == null) 
            return new BadRequestObjectResult("User does not exist");
        
        if(product.Price == null) 
            return new BadRequestObjectResult("Price cannot be null");
        
        if(product.QuantityStock == null) 
            return new BadRequestObjectResult("Quantity stock cannot be null");

        if (image != null)
        {
            var imageUrl = await _imageService.SaveImage(image);
            product.ImageUrl = imageUrl;
        }
        
        await _productRepository.CreateProduct(product);
        return new CreatedAtActionResult(nameof(CreateProduct), "Product", new { id = product.IdProduct }, product);
        
    }
}