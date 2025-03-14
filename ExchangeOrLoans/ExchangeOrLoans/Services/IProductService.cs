using ExchangeOrLoans.Dtos;
using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IProductService
{
    Task<ActionResult<Product>> CreateProduct(ProductCreateDto productCreateDto);
    Task <ActionResult<List<Product>>> GetProducts();
    Task <ActionResult<Product>> UpdateProduct(ProductCreateDto product, int id);
    Task <ActionResult<Product>> GetProductById(int id);
    
}