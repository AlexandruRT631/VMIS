namespace listing_backend.DTOs;

public class ListingDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public int SellerId { get; set; }
    public int? Price { get; set; }
    public string? Description { get; set; }
    public CarDto? Car { get; set; }
    public int? Year { get; set; }
    public int? Mileage { get; set; }
    public CategoryDto? Category { get; set; }
    public EngineDto? Engine { get; set; }
    public DoorTypeDto? DoorType { get; set; }
    public TransmissionDto? Transmission { get; set; }
    public TractionDto? Traction { get; set; }
    public ColorDto? Color { get; set; }
    public List<FeatureDto>? Features { get; set; } = [];
    public List<ListingImageDto>? ListingImages { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public bool? IsSold { get; set; }
}