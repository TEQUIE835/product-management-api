using productManagement.Domain.Entities;

namespace productManagement.Application.Interfaces.Users;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsers();
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByUsername(string username);
    Task UpdateAsync(int id, User user);
    Task DeleteAsync(int id);
}