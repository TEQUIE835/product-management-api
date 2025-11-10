namespace productManagement.Application.DTOs.Auth;

public class LoginResponseDto
{
    
    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}