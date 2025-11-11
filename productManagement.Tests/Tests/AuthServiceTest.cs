using productManagement.Application.DTOs.Auth;
using productManagement.Application.Interfaces.Security;
using productManagement.Application.Services.Auth;

namespace productManagement.Tests.Tests;

public class AuthServiceTest
{
     private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<IJwtTokenGenerator> _jwtMock = new();
    private readonly Mock<IAuthRepository> _authRepoMock = new();
    private readonly AuthService _authService;

    public AuthServiceTest()
    {
        _authService = new AuthService(_jwtMock.Object, _userRepoMock.Object,  _authRepoMock.Object);
    }

    [Fact]
    public async Task LoginAsync_ShouldThrow_WhenUserNotFound()
    {
        _userRepoMock.Setup(r => r.GetUserByUsername("john"))
            .ReturnsAsync((User?)null);

        var dto = new LoginRequestDto { Username = "john", Password = "1234" };

        Func<Task> act = async () => await _authService.LoginAsync(dto);

        await act.Should().ThrowAsync<UnauthorizedAccessException>()
            .WithMessage("Invalid username or password");
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsValid()
    {
        var user = new User
        {
            Id = 1,
            Username = "john",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234"),
            Role = "User"
        };

        _userRepoMock.Setup(r => r.GetUserByUsername("john")).ReturnsAsync(user);
        _jwtMock.Setup(j => j.GenerateAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns(("token123", DateTime.UtcNow.AddHours(1)));

        var dto = new LoginRequestDto { Username = "john", Password = "1234" };

        var result = await _authService.LoginAsync(dto);

        result.AccessToken.Should().Be("token123");
    }

    [Fact]
    public async Task RegisterAsync_ShouldThrow_WhenUsernameExists()
    {
        _userRepoMock.Setup(r => r.GetUserByUsername("john"))
            .ReturnsAsync(new User { Id = 1, Username = "john" });

        var dto = new RegisterRequestDto { Username = "john", Password = "123", Email = "mail@mail.com" };

        Func<Task> act = async () => await _authService.RegisterAsync(dto);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Username already exists");
    }

    [Fact]
    public async Task RegisterAsync_ShouldAdd_WhenValid()
    {
        _userRepoMock.Setup(r => r.GetUserByUsername("john")).ReturnsAsync((User?)null);

        var dto = new RegisterRequestDto { Username = "john", Password = "123", Email = "mail@mail.com" };

        await _authService.RegisterAsync(dto);

        _userRepoMock.Verify(r => r.AddAsync(It.Is<User>(u => u.Username == "john")), Times.Once);
    }
}