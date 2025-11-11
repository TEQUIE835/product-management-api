using Microsoft.EntityFrameworkCore;
using productManagement.Domain.Entities;
using productManagement.Domain.Interfaces;
using productManagement.Infrastructure.Persistence;

namespace productManagement.Infrastructure.Repositories;

public class AuthRepository : IAuthRepository
{
    private readonly AppDbContext _context;
    public AuthRepository(AppDbContext context)
    {
        _context = context;
    }


    public async Task AddRefreshTokenAsync(RefreshToken token)
    {
        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();
    }

    public async Task<RefreshToken?> GetRefreshTokenAsync(string token)
    {
        return await _context.RefreshTokens.Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Token == token);
    }

    public async Task RemoveRefreshTokenAsync(RefreshToken token)
    {
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
    }
}