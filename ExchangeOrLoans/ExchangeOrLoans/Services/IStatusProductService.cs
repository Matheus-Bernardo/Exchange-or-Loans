using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IStatusProductService
{
    Task<ActionResult<StatusProduct>> createStatusProduct(StatusProduct statusProduct);
    Task<ActionResult<StatusProduct>> updateStatusProduct(StatusProduct statusProduct, int id);
    Task<ActionResult<StatusProduct>> deleteStatusProduct(int id);
    Task<ActionResult<StatusProduct>> getStatusProductById(int id);
    Task<ActionResult<StatusProduct>> getStatusProduct();

}