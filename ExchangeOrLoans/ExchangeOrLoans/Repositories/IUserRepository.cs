using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;

namespace ExchangeOrLoans.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<List<UserDto>> GetAllUsers();
    Task<UserDto?> GetUserById(int id);
    Task<bool> UsernameExists(string username);
    Task<bool> EmailExists(string email);
    Task<bool> PasswordAndEmailSearch(string email, string password);
    Task<User> GetUserByEmail(string email);

}