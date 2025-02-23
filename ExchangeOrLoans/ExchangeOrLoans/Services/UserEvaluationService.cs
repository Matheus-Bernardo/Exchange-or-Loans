using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public class UserEvaluationService : IUserEvaluationService
{
    private readonly IUserEvaluationRepository _userEvaluationRepository;
    private readonly IUserRepository _userRepository;

    public UserEvaluationService(IUserEvaluationRepository userEvaluationRepository, IUserRepository userRepository)
    {
        _userEvaluationRepository = userEvaluationRepository;
        _userRepository = userRepository;
    }
    
    public async Task<ActionResult<UserEvaluation>> CreateScoreUser(UserEvaluation userEvaluation)
    {
        if (userEvaluation.EvaluatedScore == null)
        {
            return new BadRequestObjectResult("An evaluation note must be provided");
        }
            
        if (userEvaluation.EvaluatedScore <0 || userEvaluation.EvaluatedScore > 5)
        {
            return new BadRequestObjectResult("The rating must be between 0 and 5");
        }
        if (userEvaluation.IdUserRated == userEvaluation.IdUserReviewer)
        {
            return new BadRequestObjectResult("you can't evaluate yourself");
        }
        
        if (await _userRepository.GetUserById(userEvaluation.IdUserReviewer) == null)
        {
            return new BadRequestObjectResult("User appraiser not found");
        }
        
        if (await _userRepository.GetUserById(userEvaluation.IdUserRated) == null)
        {
            return new BadRequestObjectResult("User Evaluated not found");
        }

        var createdEvaluation = await _userEvaluationRepository.CreateScoreUser(userEvaluation);
        
        if (createdEvaluation == null)
        {
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
        
        return new ObjectResult(createdEvaluation) { StatusCode = StatusCodes.Status201Created };

    }
}