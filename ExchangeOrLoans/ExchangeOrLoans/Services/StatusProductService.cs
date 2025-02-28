using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public class StatusProductService : IStatusProductService
{
    private readonly IStatusProductRepository _statusProductRepository;

    public StatusProductService(IStatusProductRepository statusProductRepository)
    {
        _statusProductRepository = statusProductRepository;
    }
    
    public async Task<ActionResult<StatusProduct>> createStatusProduct(StatusProduct statusProduct)
    {
        if (statusProduct == null)
        {
            return new BadRequestObjectResult("A status product is required");
        }

        if (statusProduct.Name == null)
        {
            return new BadRequestObjectResult("A status product name is required");
        }
        
        await _statusProductRepository.CreateStatusProduct(statusProduct);
        return new OkObjectResult(statusProduct);
    }

    public async Task<ActionResult<StatusProduct>> updateStatusProduct(StatusProduct statusProduct, int id)
    {
        var statusProductFind = await _statusProductRepository.GetStatusProductById(id);
        if (statusProductFind == null) return new NotFoundObjectResult("No status product found");

        statusProductFind.Name = statusProduct.Name;
        await _statusProductRepository.UpdateStatusProduct(statusProductFind);
        return new OkObjectResult("Status product updated");

    }

    public async Task<ActionResult<StatusProduct>> deleteStatusProduct(int id)
    {
        var statusProduct = await _statusProductRepository.GetStatusProductById(id);
    
        if (statusProduct == null)
        {
            return new NotFoundObjectResult("StatusProduct not found");
        }

        var deleted = await _statusProductRepository.DeleteStatusProduct(id);
    
        if (!deleted)
        {
            return new BadRequestObjectResult("Error deleting StatusProduct");
        }

        return new OkObjectResult("StatusProduct deleted successfully");
    }

    public async Task<ActionResult<StatusProduct>> getStatusProductById(int id)
    {
        var productStatus = await _statusProductRepository.GetStatusProductById(id);
        if(productStatus == null) return new NotFoundObjectResult("No product found");
        return new OkObjectResult(productStatus);
        
    }

    public async Task<ActionResult<StatusProduct>> getStatusProduct()
    {
        var statusProducts = await _statusProductRepository.GetStatusProducts();
        if (statusProducts == null || !statusProducts.Any()) return new NotFoundObjectResult("No product found");
        return new OkObjectResult(statusProducts);
    }
}