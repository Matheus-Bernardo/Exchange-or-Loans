using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IProductService
{
    Task<ActionResult<Product>> CreateProduct(Product product,IFormFile image);
}