using productManagement.Application.DTOs.Users;
using productManagement.Domain.Entities;

namespace productManagement.Application.Interfaces.Users;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllUsers();
    Task<UserResponseDto?> GetUserById(int id);
    Task<UserResponseDto?> GetUserByUsername(string username);
    Task UpdateAsync(int id, User user);
    Task DeleteAsync(int id);
}