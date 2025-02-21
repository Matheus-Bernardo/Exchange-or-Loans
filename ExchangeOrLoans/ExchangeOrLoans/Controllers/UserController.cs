using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace ExchangeOrLoans.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> CreateUser(User user)
    {
        return await _userService.CreateUser(user);
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        return  await _userService.GetUserById(id);
        
    }
    
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        return await _userService.GetUsers();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody]LoginDto loginDto)
    {
        return await _userService.Login(loginDto);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<bool> DeleteUser(int id)
    {
        return await _userService.DeleteUser(id);
    }
    
    [Authorize]
    [HttpPatch("{id}")]
    public async Task<ActionResult<User>> UpdateUser( UserDto user, int id)
    {
        var updatedUser = await _userService.UpdateUser(user, id);
        if (updatedUser == null) return NotFound("User not found");
        
        return updatedUser;
    }
}