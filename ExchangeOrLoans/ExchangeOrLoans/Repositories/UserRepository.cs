using ExchangeOrLoans.data;
using ExchangeOrLoans.models;
using Microsoft.EntityFrameworkCore;

namespace ExchangeOrLoans.Repositories;

public class UserRepository: IUserRepository
{
    //Creates an object that allows connection and manipulation of the database
    private readonly ApplicationDbContext _dbContext;
    
    public UserRepository(ApplicationDbContext dbContext)
    {
        this._dbContext = dbContext;
    }
    
    public async Task<User> CreateUser(User user)
    {
        _dbContext.User.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }
    public async Task<User?> GetUserById(int id)
    {
        return await _dbContext.User.FindAsync(id);
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await _dbContext.User.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _dbContext.User.AnyAsync(u => u.Email == email);
    }
    
    
}