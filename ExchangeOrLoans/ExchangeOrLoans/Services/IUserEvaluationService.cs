using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IUserEvaluationService
{
    Task<ActionResult<UserEvaluation>> CreateScoreUser(UserEvaluation userEvaluation);
}