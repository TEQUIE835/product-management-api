using productManagement.Application.Interfaces.Users;
using productManagement.Domain.Entities;
using productManagement.Domain.Interfaces;

namespace productManagement.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _userRepository.GetAllUsers();
    }

    public async Task<User?> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null) throw new ArgumentException("User not found");
        return user;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null) throw new ArgumentException("User not found");
        return user;
    }

    public async Task UpdateAsync(int id, User user)
    {
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser == null) throw new ArgumentException("User not found");
        var existingWithSameUsername = await _userRepository.GetUserByUsername(user.Username);
        if (existingWithSameUsername != null && existingWithSameUsername.Id != id) throw new InvalidOperationException("Username already exists");
        existingUser.Username = user.Username;
        existingUser.Email = user.Email;
        existingUser.Role = user.Role;
        await _userRepository.UpdateAsync(existingUser);
    }
    public async Task DeleteAsync(int id)
    {
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser == null) throw new ArgumentException("User not found");
        await _userRepository.DeleteAsync(existingUser);
    }
}