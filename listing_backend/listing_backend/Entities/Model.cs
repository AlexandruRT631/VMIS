using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Model
{
    [Key]
    public int Id { get; set; }
    public Make Make { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public List<Category> PossibleCategories { get; set; } = [];
    public List<DoorType> PossibleDoorTypes { get; set; } = [];
    public List<Transmission> PossibleTransmissions { get; set; } = [];
    public List<Traction> PossibleTractions { get; set; } = [];
    public List<Engine> PossibleEngines { get; set; } = [];
    public List<Listing> Listings { get; set; } = [];
}