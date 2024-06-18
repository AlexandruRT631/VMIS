namespace listing_backend.DTOs;

public class ListingSearchDto
{
    public string? Keywords { get; set; }
    public int? MakeId { get; set; }
    public int? ModelId { get; set; }
    public int? StartYear { get; set; }
    public int? MinYear { get; set; }
    public int? MaxYear { get; set; }
    public int? MinMileage { get; set; }
    public int? MaxMileage { get; set; }
    public List<int>? Categories { get; set; } = [];
    public List<int>? Fuels { get; set; } = [];
    public int? MinPower { get; set; }
    public int? MaxPower { get; set; }
    public int? MinTorque { get; set; }
    public int? MaxTorque { get; set; }
    public int? MinDisplacement { get; set; }
    public int? MaxDisplacement { get; set; }
    public string? EngineCode { get; set; }
    public List<int>? DoorTypes { get; set; } = [];
    public List<int>? Transmissions { get; set; } = [];
    public List<int>? Tractions { get; set; } = [];
    public List<int>? Colors { get; set; } = [];
    public List<int>? Features { get; set; } = [];
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
}