using ExchangeOrLoans.models;
using ExchangeOrLoans.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserEvaluationController : ControllerBase
{
    private readonly IUserEvaluationService _evaluationService;

    public UserEvaluationController(IUserEvaluationService evaluationService)
    {
        _evaluationService = evaluationService;
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    
    public async Task<ActionResult<UserEvaluation>> CreateScoreUser(UserEvaluation userEvaluation)
    {
        return await _evaluationService.CreateScoreUser(userEvaluation);
    }
}