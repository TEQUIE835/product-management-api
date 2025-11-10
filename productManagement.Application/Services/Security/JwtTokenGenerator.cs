using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using productManagement.Application.Interfaces.Security;

namespace productManagement.Application.Services.Security;

public class JwtTokenGenerator :IJwtTokenGenerator
{
    private readonly IConfiguration _configuration;
    public JwtTokenGenerator(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public (string token, DateTime expiresAt) GenerateToken(string userId, string username, string role)
    {
        
        var jwtKey = _configuration["Jwt:Key"] 
                     ?? Environment.GetEnvironmentVariable("SECRET_KEY")
                     ?? throw new InvalidOperationException("JWT Key is missing.");

        var jwtIssuer = _configuration["Jwt:Issuer"] 
                        ?? Environment.GetEnvironmentVariable("ISSUER");

        var jwtAudience = _configuration["Jwt:Audience"] 
                          ?? Environment.GetEnvironmentVariable("AUDIENCE");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiresAt = DateTime.UtcNow.AddHours(2);
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: creds
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return (tokenString, expiresAt);
    }
}