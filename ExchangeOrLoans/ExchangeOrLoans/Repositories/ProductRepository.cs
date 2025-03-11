using ExchangeOrLoans.data;
using ExchangeOrLoans.models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeOrLoans.Repositories;

public class ProductRepository : IProductRepository

{
    private readonly ApplicationDbContext _dbContext ;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Product> CreateProduct(Product product)
    {
        _dbContext.Product.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> UpdateProduct(Product product)
    {
        _dbContext.Product.Update(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetProductById(int id)
    {
        var product = await _dbContext.Product.Where(product => product.IdProduct == id).FirstOrDefaultAsync();
        if(product == null) return null;
        return product;
    }

    public async Task<List<Product>> GetProducts()
    {
        return await _dbContext.Product.ToListAsync();
    }

   
}