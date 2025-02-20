using BCrypt.Net;
using ExchangeOrLoans.DTOS;
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
        
        if (string.IsNullOrEmpty(user.Password))
        {
            return new BadRequestObjectResult("Password is required");
        }
        
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.Password = passwordHash;
        
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
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users =  await _userRepository.GetAllUsers();
        if (users == null || !users.Any())
        {
            return new NotFoundObjectResult("Users not found");
        }
        
        return new OkObjectResult(users);
    }
    public async Task<ActionResult<string>> Login(LoginDto loginDto)
    {
        var user = await _userRepository.GetUserByEmail(loginDto.Email);
        
        if (user == null) return new BadRequestObjectResult("Email not found");
        
        bool passwordMatches = BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password);
        if (!passwordMatches) return new BadRequestObjectResult("Password Incorrect");
        
        return new OkObjectResult(new {message = "Login successful", nome= user.FirstName});
        
    }
    public async Task<ActionResult<User>> UpdateUser(UserDto user, int id)
    {
        var userFind = await _userRepository.GetUserById(id);
        if (userFind == null) return new NotFoundObjectResult("User not found");
        
        userFind.Username = user.Username ?? userFind.Username;
        userFind.Email = user.Email ?? userFind.Email;
        userFind.FirstName = user.FirstName ?? userFind.FirstName;
        userFind.LastName = user.LastName ?? userFind.LastName;
        userFind.Score = user.Score ?? userFind.Score;
        
        if (!string.IsNullOrEmpty(user.Password))
        {
            userFind.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }
        
        return new OkObjectResult($"User updated Successfully");
    }
    public async  Task<bool> DeleteUser(int id)
    {
        var userDto = await _userRepository.GetUserById(id);
        if (userDto == null) return false;

        return await _userRepository.DeleteUser(id);
    }
}