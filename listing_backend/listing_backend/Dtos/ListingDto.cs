namespace listing_backend.DTOs;

public class ListingDto
{
    public int Id { get; set; }
    public int SellerId { get; set; }
    public int Price { get; set; }
    public CarDto? Car { get; set; }
    public int Year { get; set; }
    public int Mileage { get; set; }
    public CategoryDto? Category { get; set; }
    public EngineDto? Engine { get; set; }
    public DoorTypeDto? DoorType { get; set; }
    public TransmissionDto? Transmission { get; set; }
    public TractionDto? Traction { get; set; }
    public ColorDto? InteriorColor { get; set; }
    public ColorDto? ExteriorColor { get; set; }
    public List<FeatureExteriorDto>? ExteriorFeatures { get; set; } = [];
    public List<FeatureInteriorDto>? InteriorFeatures { get; set; } = [];
}