using AutoMapper;
using user_backend.DTOs;
using user_backend.Entities;

namespace user_backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
    }
}