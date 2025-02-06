using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;

namespace ExchangeOrLoans.Repositories;

public interface IUserRepository
{
    Task<User> CreateUser(User user);
    Task<List<UserDto>> GetAllUsers();
    Task<User?> GetUserById(int id);
    Task<bool> UsernameExists(string username);
    Task<bool> EmailExists(string email);
    Task<bool> DeleteUser(int id);
    Task<User> GetUserByEmail(string email);
    Task<User> UpdateUser(User user);

}