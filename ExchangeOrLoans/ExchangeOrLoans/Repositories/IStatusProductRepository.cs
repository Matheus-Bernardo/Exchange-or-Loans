using ExchangeOrLoans.models;

namespace ExchangeOrLoans.Repositories;

public interface IStatusProductRepository
{
    Task<StatusProduct> CreateStatusProduct(StatusProduct statusProduct);
    Task<StatusProduct> UpdateStatusProduct(StatusProduct statusProduct);
    Task<bool> DeleteStatusProduct(int IdStatusProduct);
    Task<StatusProduct?> GetStatusProductById(int id);
    Task<List<StatusProduct>> GetStatusProducts();
}