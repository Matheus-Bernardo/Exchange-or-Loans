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
}