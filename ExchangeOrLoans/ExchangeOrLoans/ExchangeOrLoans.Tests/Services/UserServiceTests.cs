using ExchangeOrLoans.DTOS;
using ExchangeOrLoans.models;
using ExchangeOrLoans.Repositories;
using ExchangeOrLoans.Services;
namespace ExchangeOrLoans.ExchangeOrLoans.Tests.Services;

using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateUser_ValidUser_ShouldReturnCreatedUser()
    {
        var user = new User
        {
            Username = "exampleUser",
            Email = "example@example.com",
            FirstName = "Example",
            LastName = "Example2",
            Password = "securePassword",
            Score = 1
        };
        user.Score = 1;
        user.CreatedAt = DateTime.UtcNow;

        _userRepositoryMock.Setup(repo => repo.UsernameExists(user.Username)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repo => repo.EmailExists(user.Email)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(user);
        
        var result = await _userService.CreateUser(user);

        Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdAtAction = result.Result as CreatedAtActionResult;
        Assert.Equal("CreateUser", createdAtAction.ActionName);
        Assert.Equal(user.Id, ((User)createdAtAction.Value).Id);
        
    }
    
    [Fact]
    public async Task UpdateUser_ValidUser_ShouldReturnUpdatedUser()
    {
        var user = new User
        {
            Id = 1,
            Username = "exampleUser",
            Email = "example@example.com",
            FirstName = "Example",
            LastName = "Example2",
            Password = "securePassword",
            Score = 1,
            
        };
        
        var updateUserDto = new UserDto
        {
            Id = 1,
            Username = "updateUser",
            Email = "update@example.com",
            FirstName = "ExampleUpdate",
            LastName = "Example2Update",
            Password = "securePasswordUpdate",
            Score = 5,
            
        };
        
        var updatedUser = new User
        {
            Id = user.Id,
            Username = updateUserDto.Username,
            Email = updateUserDto.Email,
            FirstName = updateUserDto.FirstName,
            LastName = updateUserDto.LastName,
            Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password),
            Score = updateUserDto.Score ?? user.Score, // Trata valores nulos
        };
        
        //Simulates the user ID search in the bank
        _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.UpdateUser(It.IsAny<User>())).ReturnsAsync(updatedUser);
        
        var result = await _userService.UpdateUser(updateUserDto,user.Id);
        
        Assert.IsType<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        Assert.Equal(200, okResult.StatusCode);

        var returnedUser = okResult.Value as string;
        Assert.NotNull(returnedUser);
        Assert.Equal("User updated Successfully", returnedUser);
        
    }

    [Fact]
    public async Task DeleteUser_ValidUser_ShouldReturnTrue()
    {
        var user = new User
        {
            Id = 1,
            Username = "exampleUser",
            Email = "example@example.com",
            FirstName = "Example",
            LastName = "Example2",
            Password = "securePassword",
            Score = 1
        };
        _userRepositoryMock.Setup(repo => repo.GetUserById(user.Id)).ReturnsAsync(user);
        _userRepositoryMock.Setup(repo => repo.DeleteUser(user.Id)).ReturnsAsync(true);
        
        var result = await _userService.DeleteUser(user.Id);
        
        Assert.True(result);
    }
    
    [Fact]
    public async Task GetUsers_WhenUsersExist_ShouldReturnOkWithUsers()
    {
        var users = new List<UserDto>
        {
            new UserDto { Id = 1, FirstName = "John Doe", Email = "john@example.com" },
            new UserDto { Id = 2, FirstName = "Jane Doe", Email = "jane@example.com" }
        };

        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(users);

        var result = await _userService.GetUsers();
        
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedUsers = Assert.IsType<List<UserDto>>(okResult.Value);
        Assert.Equal(2, returnedUsers.Count);
    }
    
    [Fact]
    public async Task GetUsers_WhenNoUsersExist_ShouldReturnNotFound()
    {
        _userRepositoryMock.Setup(repo => repo.GetAllUsers()).ReturnsAsync(new List<UserDto>());
        
        var result = await _userService.GetUsers();
        
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        Assert.Equal("Users not found", notFoundResult.Value);
    }

    [Fact]
    public async Task LoginUser_InvalidPassword_ShouldReturnBadRequest()
    {
      
        var loginDto = new LoginDto
        {
            Email = "example@example.com",
            Password = "wrongPassword"
        };

        var user = new User
        {
            Username = "exampleUser",
            Email = "example@example.com",
            FirstName = "Example",
            LastName = "Example2",
            Password = BCrypt.Net.BCrypt.HashPassword("correctPassword"),
            Score = 1,
            CreatedAt = DateTime.UtcNow
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(loginDto.Email)).ReturnsAsync(user);

        var service = new UserService(_userRepositoryMock.Object);
        
        var result = await service.Login(loginDto);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Password Incorrect", badRequestResult.Value);
    }
    
    [Fact]
    public async Task LoginUser_InvalidEmail_ShouldReturnBadRequest()
    {
       
        var loginDto = new LoginDto
        {
            Email = "nonexistent@example.com",
            Password = "anyPassword"
        };

      
        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(loginDto.Email)).ReturnsAsync((User)null!);

        var service = new UserService(_userRepositoryMock.Object);
        
        var result = await service.Login(loginDto);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Email not found", badRequestResult.Value);
    }

    [Fact]
    public async Task CreateUser_UsernameAlreadyExists_ShouldReturnBadRequest()
    {
        var newUser = new User
        {
            Username = "existingUser",
            Email = "newuser@example.com",
            Password = "validPassword123",
            FirstName = "New",
            LastName = "User"
        };

        _userRepositoryMock.Setup(repo => repo.UsernameExists(newUser.Username)).ReturnsAsync(true);

        var service = new UserService(_userRepositoryMock.Object);
        
        var result = await service.CreateUser(newUser);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Username already exists", badRequestResult.Value);
    }
    
    [Fact]
    public async Task CreateUser_EmailAlreadyExists_ShouldReturnBadRequest()
    {
       
        var newUser = new User
        {
            Username = "newusername",
            Email = "existing@example.com",
            Password = "validPassword123",
            FirstName = "New",
            LastName = "User"
        };

        _userRepositoryMock.Setup(repo => repo.UsernameExists(newUser.Username)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repo => repo.EmailExists(newUser.Email)).ReturnsAsync(true);

        var service = new UserService(_userRepositoryMock.Object);
        
        var result = await service.CreateUser(newUser);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Email already exists", badRequestResult.Value);
    }
    
    [Fact]
    public async Task CreateUser_PasswordIsNullOrEmpty_ShouldReturnBadRequest()
    {
        
        var newUser = new User
        {
            Username = "uniqueUser",
            Email = "unique@example.com",
            Password = "",
            FirstName = "Empty",
            LastName = "Password"
        };

        _userRepositoryMock.Setup(repo => repo.UsernameExists(newUser.Username)).ReturnsAsync(false);
        _userRepositoryMock.Setup(repo => repo.EmailExists(newUser.Email)).ReturnsAsync(false);

        var service = new UserService(_userRepositoryMock.Object);
        
        var result = await service.CreateUser(newUser);
        
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Password is required", badRequestResult.Value);
    }
}