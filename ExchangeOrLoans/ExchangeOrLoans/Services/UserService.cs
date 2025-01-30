using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeOrLoans.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ActionResult<User>> CreateUser(User user)
    {
        if (await _userRepository.UsernameExists(user.Username))
        {
            return new BadRequestObjectResult("Username already exists");
        }

        if (await _userRepository.EmailExists(user.Email))
        {
            return new BadRequestObjectResult("Email already exists");
        }
        
        var createdUser = await _userRepository.CreateUser(user);
        return new CreatedAtActionResult(nameof(CreateUser), "User", new { id = createdUser.Id }, createdUser);

    }
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return new NotFoundObjectResult("User not found");
        }

        return new OkObjectResult(user);
    }
    
     
    
}