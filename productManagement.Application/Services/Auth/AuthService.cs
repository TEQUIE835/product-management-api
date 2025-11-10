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

    public AuthService(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }


    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user = await _userRepository.GetUserByUsername(request.Username);
        if (user == null)throw new UnauthorizedAccessException("Invalid username or password");
        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash)) throw new UnauthorizedAccessException("Invalid username or password");
        var (token, expiresAt) = _jwtTokenGenerator.GenerateToken(user.Id.ToString(), user.Username, user.Role);
        return new LoginResponseDto()
        {
            Token = token,
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
}