using ExchangeOrLoans.models;
using ExchangeOrLoans.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class StatusProductController : ControllerBase
{
    private readonly IStatusProductService _statusProductService;

    public StatusProductController(IStatusProductService statusProductService)
    {
        _statusProductService = statusProductService;
    }

    [Authorize]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusProduct>> GetStatusProduct()
    {
        return await _statusProductService.getStatusProduct();
    }

    [Authorize]
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusProduct>> getStatusProductById(int id)
    {
        return await _statusProductService.getStatusProductById(id);
    }
    
    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusProduct>> deleteStatusProduct(int id)
    {
        return await _statusProductService.deleteStatusProduct(id);
    }

    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPut("{id}")]
    public async Task<ActionResult<StatusProduct>> updateStatusProduct(StatusProduct statusProduct, int id)
    {
        var updateStatusProduct = await _statusProductService.updateStatusProduct(statusProduct, id);
        if(updateStatusProduct == null) return NotFound("StatusProduct not found");
        
        return updateStatusProduct;
    }
    
    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<StatusProduct>> createStatusProduct(StatusProduct statusProduct)
    {
        return await _statusProductService.createStatusProduct(statusProduct);
    }
    
}