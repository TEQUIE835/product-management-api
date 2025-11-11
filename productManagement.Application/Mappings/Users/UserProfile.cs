using AutoMapper;
using productManagement.Application.DTOs.Users;
using productManagement.Domain.Entities;

namespace productManagement.Application.Mappings.Users;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponseDto>();
    }
}