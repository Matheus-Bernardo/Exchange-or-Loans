using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IUserService
{
    Task<ActionResult<User>> CreateUser(User user);
    Task<ActionResult<List<UserDto>>> GetUsers();
    Task<ActionResult<User>> GetUserById(int id);
    Task<ActionResult<string>> Login(LoginDto loginDto);
    
}