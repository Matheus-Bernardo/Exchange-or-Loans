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

    public async Task<ActionResult<List<Product>>> GetProducts()
    {
        var products = await _productRepository.GetProducts();
        
        if(products == null || !products.Any()) return new NotFoundObjectResult("No products found");
        return new OkObjectResult(products);
    }

    public async Task<ActionResult<Product>> UpdateProduct(Product product,IFormFile? image, int id)
    {
        var productFind = await _productRepository.GetProductById(id);
        if(productFind == null) return new NotFoundObjectResult("Product not found");

        if (image != null)
        {
            var imageUrl = await _imageService.SaveImage(image);
            productFind.ImageUrl = imageUrl;
        }
        
        productFind.Name = product.Name?? productFind.Name;
        productFind.Price = product.Price ?? productFind.Price;
        productFind.QuantityStock = product.QuantityStock ?? productFind.QuantityStock;
        productFind.ImageUrl = product.ImageUrl ?? productFind.ImageUrl;
        productFind.Description = product.Description ?? productFind.Description;
        productFind.UpdateAt = DateTime.UtcNow;
        productFind.IdStatusProduct =product.IdStatusProduct ?? productFind.IdStatusProduct;

        try
        {
            await _productRepository.UpdateProduct(productFind);
            return new OkObjectResult(new { message = "Product updated", product = productFind });

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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