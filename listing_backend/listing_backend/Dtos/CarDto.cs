namespace listing_backend.DTOs;

public class CarDto
{
    public int Id { get; set; }
    public ModelDto? Model { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public List<CategoryDto>? PossibleCategories { get; set; } = [];
    public List<DoorTypeDto>? PossibleDoorTypes { get; set; } = [];
    public List<TransmissionDto>? PossibleTransmissions { get; set; } = [];
    public List<TractionDto>? PossibleTractions { get; set; } = [];
    public List<EngineDto>? PossibleEngines { get; set; } = [];
}