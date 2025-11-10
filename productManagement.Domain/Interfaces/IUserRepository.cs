using productManagement.Domain.Entities;

namespace productManagement.Domain.Interfaces;

public interface IUserRepository
{
    public Task<List<User>> GetAllUsers();
    public Task<User?> GetUserById(int id);
    public Task<User?> GetUserByUsername(string username);
    public Task AddAsync(User user);
    public Task UpdateAsync(User user);
    public Task DeleteAsync(User user);
}