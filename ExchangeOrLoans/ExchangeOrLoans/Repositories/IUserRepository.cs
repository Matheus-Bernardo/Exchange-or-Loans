using ExchangeOrLoans.models;

namespace ExchangeOrLoans.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<User?> GetUserById(int id);
    Task<bool> UsernameExists(string username);
    Task<bool> EmailExists(string email);
    
}