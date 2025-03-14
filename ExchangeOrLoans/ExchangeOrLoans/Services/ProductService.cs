using ExchangeOrLoans.Dtos;
using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Utils;
using Microsoft.AspNetCore.Mvc;
using Exception = System.Exception;

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
    
    public async Task<ActionResult<Product>> CreateProduct(ProductCreateDto productDto)
    {
        if (productDto == null) 
            return new BadRequestObjectResult("Product cannot be null");
        
        if(await _userRepository.GetUserById(productDto.IdUserSeller) == null) 
            return new BadRequestObjectResult("User does not exist");
        
        if(productDto.Price == null) 
            return new BadRequestObjectResult("Price cannot be null");
        
        if(productDto.QuantityStock == null) 
            return new BadRequestObjectResult("Quantity stock cannot be null");
        
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            IdUserSeller = productDto.IdUserSeller,
            IdUserBuyer = productDto.IdUserBuyer,
            Price = productDto.Price,
            IdStatusProduct = productDto.IdStatusProduct,
            QuantityStock = productDto.QuantityStock,
            CreatedAt = DateTime.UtcNow
        };

        if (productDto.Image != null)
        {
            try
            {
                product.ImageUrl = await _imageService.SaveImage(productDto.Image);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Image upload failed: {ex.Message}");
            }
        }
        
        await _productRepository.CreateProduct(product);
        return new CreatedAtActionResult(nameof(CreateProduct), "Product", new { id = product.IdProduct }, product);
        
    }

    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        
        if(products == null || !products.Any()) return new NotFoundObjectResult("No products found");
        return new OkObjectResult(products);
    }

    public async Task<ActionResult<Product>> UpdateProduct(ProductCreateDto productDto, int id)
    {
        var productFind = await _productRepository.GetProductById(id);
        if(productFind == null) return new NotFoundObjectResult("Product not found");
        
        productFind.Name = productDto.Name ?? productFind.Name;
        productFind.Price = productDto.Price;
        productFind.QuantityStock = productDto.QuantityStock;
        productFind.Description = productDto.Description ?? productFind.Description;
        productFind.UpdateAt = DateTime.UtcNow;

        if (productDto.Image != null)
        {
            try
            {
                productFind.ImageUrl = await _imageService.SaveImage(productDto.Image);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Image upload failed: {ex.Message}");
            }
        }
        
        try
        {
            await _productRepository.UpdateProduct(productFind);
            return new OkObjectResult(new { message = "Product updated", product = productFind });

        }
        catch (Exception e)
        {
            return new NotFoundObjectResult("Product not updated");
        }
        
    }

    public async Task<ActionResult<Product>> GetProductById(int id)
    {
        var productFind = await _productRepository.GetProductById(id);
        if (productFind == null) return new NotFoundObjectResult("Product not found");
        return new OkObjectResult(productFind);
    }
}