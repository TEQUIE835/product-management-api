using productManagement.Domain.Entities;

namespace productManagement.Domain.Interfaces;

public interface IAuthRepository
{
    Task AddRefreshTokenAsync(RefreshToken token);
    Task<RefreshToken?> GetRefreshTokenAsync(string token);
    Task RemoveRefreshTokenAsync(RefreshToken token);
}