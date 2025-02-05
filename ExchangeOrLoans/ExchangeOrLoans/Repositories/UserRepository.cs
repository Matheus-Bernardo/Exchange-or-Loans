using ExchangeOrLoans.data;
using ExchangeOrLoans.DTOS;
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

    public async Task<List<UserDto>> GetAllUsers()
    {
        return await _dbContext.User.Select(user => new UserDto {Id= user.Id, Username = user.Username, FirstName = user.FirstName }).ToListAsync();
    }
    public async Task<UserDto?> GetUserById(int id)
    {
        var user = await _dbContext.User.Where(user => user.Id == id).Select(user => new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Score = user.Score,
            CreatedAt = user.CreatedAt,

        }).FirstOrDefaultAsync();

        if (user == null)
        {
            return null;
        }

        return user;
      
    }

    public async Task<bool> UsernameExists(string username)
    {
        return await _dbContext.User.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> EmailExists(string email)
    {
        return await _dbContext.User.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> DeleteUser(int id)
    {
        var user = await _dbContext.User.FindAsync(id);
        if(user == null) return false;
        
        _dbContext.User.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetUserByEmail(string email)
    {
            return await _dbContext.User.FirstOrDefaultAsync(u => u.Email == email);
    }

   
}