using productManagement.Application.DTOs;
using productManagement.Application.DTOs.Auth;

namespace productManagement.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto request);
    Task RegisterAsync(RegisterRequestDto request);
    Task<string?> RefreshAccessToken(string refreshToken);
    Task<bool> LogoustAsync(string refreshToken);
}