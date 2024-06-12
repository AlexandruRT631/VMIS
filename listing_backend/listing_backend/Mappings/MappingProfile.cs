using AutoMapper;
using listing_backend.DTOs;
using listing_backend.Entities;

namespace listing_backend.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>()
            .ReverseMap();
        CreateMap<Color, ColorDto>()
            .ReverseMap();
        CreateMap<DoorType, DoorTypeDto>()
            .ReverseMap();
        CreateMap<Feature, FeatureDto>()
            .ReverseMap();
        CreateMap<Fuel, FuelDto>()
            .ReverseMap();
        CreateMap<Make, MakeDto>()
            .ReverseMap();
        CreateMap<Traction, TractionDto>()
            .ReverseMap();
        CreateMap<Transmission, TransmissionDto>()
            .ReverseMap();
        CreateMap<Model, ModelDto>()
            .ReverseMap();
        CreateMap<Engine, EngineDto>()
            .ReverseMap();
        CreateMap<Car, CarDto>()
            .ReverseMap();
        CreateMap<Listing, ListingDto>()
            .ReverseMap();
        CreateMap<ListingImage, ListingImageDto>()
            .ReverseMap();
    }
}