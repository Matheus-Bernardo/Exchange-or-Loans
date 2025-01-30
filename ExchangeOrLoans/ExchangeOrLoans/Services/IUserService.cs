using ExchangeOrLoans.models;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public interface IUserService
{
    Task<ActionResult<User>> CreateUser(User user);
    Task<ActionResult<User>> GetUserById(int id);
}