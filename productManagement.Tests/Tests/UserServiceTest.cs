using productManagement.Application.Services.Users;

namespace productManagement.Tests.Tests;

public class UserServiceTest
{
     private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _userService = new UserService(_userRepoMock.Object);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenUserNotFound()
    {
        _userRepoMock.Setup(r => r.GetUserById(1)).ReturnsAsync((User?)null);

        Func<Task> act = async () => await _userService.UpdateAsync(1, new User());

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("User not found");
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrow_WhenUsernameAlreadyExists()
    {
        var existing = new User { Id = 1, Username = "old" };
        var conflict = new User { Id = 2, Username = "new" };

        _userRepoMock.Setup(r => r.GetUserById(1)).ReturnsAsync(existing);
        _userRepoMock.Setup(r => r.GetUserByUsername("new")).ReturnsAsync(conflict);

        Func<Task> act = async () => await _userService.UpdateAsync(1, new User { Username = "new" });

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Username already exists");
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdate_WhenValid()
    {
        var existing = new User { Id = 1, Username = "old" };
        _userRepoMock.Setup(r => r.GetUserById(1)).ReturnsAsync(existing);
        _userRepoMock.Setup(r => r.GetUserByUsername(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        var updated = new User { Username = "new", Email = "new@mail.com", Role = "Admin" };

        await _userService.UpdateAsync(1, updated);

        _userRepoMock.Verify(r => r.UpdateAsync(It.Is<User>(u => u.Username == "new")), Times.Once);
    }
}