using AutoMapper;
using productManagement.Application.DTOs.Users;
using productManagement.Application.Interfaces.Users;
using productManagement.Domain.Entities;
using productManagement.Domain.Interfaces;

namespace productManagement.Application.Services.Users;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserResponseDto>> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        return _mapper.Map<IEnumerable<UserResponseDto>>(users);
    }

    public async Task<UserResponseDto?> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null) throw new ArgumentException("User not found");
        return _mapper.Map<UserResponseDto>(user);
    }

    public async Task<UserResponseDto?> GetUserByUsername(string username)
    {
        var user = await _userRepository.GetUserByUsername(username);
        if (user == null) throw new ArgumentException("User not found");
        return _mapper.Map<UserResponseDto>(user);
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