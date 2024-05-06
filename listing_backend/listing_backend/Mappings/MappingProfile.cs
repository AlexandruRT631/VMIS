using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;

namespace listing_backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Color, ColorDto>()
            .ReverseMap();
    }
}