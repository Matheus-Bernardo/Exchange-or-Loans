using ExchangeOrLoans.models;

namespace ExchangeOrLoans.Repositories;

public interface IUserEvaluationRepository
{
    Task <UserEvaluation> CreateScoreUser(UserEvaluation newUserEvaluation);
}