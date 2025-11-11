using System.Security.Cryptography;
using productManagement.Application.DTOs;
using productManagement.Application.DTOs.Auth;
using productManagement.Application.Interfaces.Auth;
using productManagement.Application.Interfaces.Security;
using productManagement.Domain.Entities;
using productManagement.Domain.Interfaces;

namespace productManagement.Application.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;

    public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository, IAuthRepository authRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
        _authRepository = authRepository;
    }


    public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetUserByUsername(request.Username);
        if (user == null)throw new UnauthorizedAccessException("Invalid username or password");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid username or password");
        var (token, expiresAt) = _jwtTokenGenerator.GenerateAccessToken(user.Id.ToString(), user.Username, user.Role);
        var refreshToken = new RefreshToken()
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            UserId = user.Id
        };
        await  _authRepository.AddRefreshTokenAsync(refreshToken);
        return new LoginResponseDto()
        {
            AccessToken = token,
            RefreshToken = refreshToken.Token,
            ExpiresAt = expiresAt
        };
    }
    

    public async Task RegisterAsync(RegisterRequestDto request)
    {
        var existingUser = await _userRepository.GetUserByUsername(request.Username);
        if (existingUser != null) throw new InvalidOperationException("Username already exists");
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var newUser = new User()
        {
            Username = request.Username,
            PasswordHash = hashedPassword,
            Email = request.Email,
            Role = "User"
        };
        await _userRepository.AddAsync(newUser);
    }

    public async Task<string?> RefreshAccessToken(string refreshToken)
    {
        var refresh = await _authRepository.GetRefreshTokenAsync(refreshToken);
        if (refresh == null || refresh.IsExpired) throw new UnauthorizedAccessException("Not valid token");
        var (accessToken, _) = _jwtTokenGenerator.GenerateAccessToken(
            refresh.UserId.ToString(),
            refresh.User.Username,
            refresh.User.Role
        );
        return accessToken;
    }
    
    public async Task<bool> LogoustAsync(string refreshToken)
    {
        var storedToken = await _authRepository.GetRefreshTokenAsync(refreshToken);
        if (storedToken == null) throw new UnauthorizedAccessException("Token doesn't exist");
        await _authRepository.RemoveRefreshTokenAsync(storedToken);
        return true;
    }
}