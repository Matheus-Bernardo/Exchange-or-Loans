using ExchangeOrLoans.models;

namespace ExchangeOrLoans.Repositories;

public interface IProductRepository
{
    Task<Product> CreateProduct(Product product);
    Task<Product> UpdateProduct(Product product);
    Task<Product?> GetProductById(int id);
    Task<List<Product>> GetProducts();
    
}