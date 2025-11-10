namespace productManagement.Application.Interfaces.Security;

public interface IJwtTokenGenerator
{
    (string token, DateTime expiresAt) GenerateToken(string userId, string username, string role);
}