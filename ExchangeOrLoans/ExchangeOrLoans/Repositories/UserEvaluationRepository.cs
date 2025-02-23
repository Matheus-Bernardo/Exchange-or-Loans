using ExchangeOrLoans.data;
using ExchangeOrLoans.models;


namespace ExchangeOrLoans.Repositories;

public class UserEvaluationRepository : IUserEvaluationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserEvaluationRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task<UserEvaluation> CreateScoreUser(UserEvaluation newUserEvaluation)
    {
        try
        {
            _dbContext.UserEvaluation.Add(newUserEvaluation);
            await _dbContext.SaveChangesAsync();
            return newUserEvaluation;
        }
        catch (Exception e)
        {
            return null;
        }
        
    }
}