using ExchangeOrLoans.data;
using ExchangeOrLoans.models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeOrLoans.Repositories;

public class StatusProductRepository : IStatusProductRepository

{
    private readonly ApplicationDbContext _dbContext;

    public StatusProductRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }
    
    public async Task<StatusProduct> CreateStatusProduct(StatusProduct statusProduct)
    {
        _dbContext.StatusProduct.Add(statusProduct);
        await _dbContext.SaveChangesAsync();
        return statusProduct;
    }

    public async Task<StatusProduct> UpdateStatusProduct(StatusProduct statusProduct)
    {
        _dbContext.StatusProduct.Update(statusProduct);
        await _dbContext.SaveChangesAsync();
        return statusProduct;
    }

    public async Task<bool> DeleteStatusProduct(int IdStatusProduct)
    {
        var product = await _dbContext.StatusProduct.FindAsync(IdStatusProduct);
        if(product == null) return false;
        
        _dbContext.StatusProduct.Remove(product);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<StatusProduct> GetStatusProductById(int id)
    {
        var product = await _dbContext.StatusProduct.Where(product => product.Id == id).FirstOrDefaultAsync();
        if(product == null) return null;
        return product;
    }

    public async Task<List<StatusProduct>> GetStatusProducts()
    {
        return await _dbContext.StatusProduct.ToListAsync();
    }
}