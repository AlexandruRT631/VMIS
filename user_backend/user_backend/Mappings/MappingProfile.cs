using AutoMapper;
using user_backend.DTOs;
using user_backend.Entities;

namespace user_backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDetailsDto>()
            .ForMember(
                dest => dest.FavouriteListings,
                opt =>
                    opt.MapFrom(
                        src => src.FavouriteListings!.Select(
                            fl => fl.FavouriteListingId
                        ).ToList()
                    )
            )
            .ForMember(
                dest => dest.FavouriteUsers,
                opt =>
                    opt.MapFrom(
                        src => src.FavouriteUsers!.Select(
                            fu => fu.FavouriteUserId
                        ).ToList()
                    )
            )
            .ForMember(
                dest => dest.Role,
                opt =>
                    opt.MapFrom(
                        src => src.Role.ToString()
                    )
            );
        CreateMap<User, UserDto>()
            .ReverseMap();
        CreateMap<Conversation, ConversationDto>()
            .ReverseMap();
        CreateMap<Message, MessageDto>()
            .ReverseMap();
    }
}