using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IProductService
{
    Task<ActionResult<Product>> CreateProduct(Product product,IFormFile image);
    Task <ActionResult<List<Product>>> GetProducts();
    Task <ActionResult<Product>> UpdateProduct(Product product,IFormFile? image ,int id);
    Task <ActionResult<Product>> GetProductById(int id);
    
}