using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Car
{
    [Key]
    public int Id { get; set; }
    public virtual Model? Model { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public virtual List<Category>? PossibleCategories { get; set; } = [];
    public virtual List<DoorType>? PossibleDoorTypes { get; set; } = [];
    public virtual List<Transmission>? PossibleTransmissions { get; set; } = [];
    public virtual List<Traction>? PossibleTractions { get; set; } = [];
    public virtual List<Engine>? PossibleEngines { get; set; } = [];
    public virtual List<Listing> Listings { get; set; } = [];
}