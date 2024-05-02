using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Engine
{
    [Key]
    public int Id { get; set; }
    public Make Make { get; set; } = null!;
    public string EngineCode { get; set; } = null!;
    public int Displacement { get; set; }
    public Fuel Fuel { get; set; } = null!;
    public int Power { get; set; }
    public int Torque { get; set; }
    public List<Car> PossibleCars { get; set; } = [];
    public List<Listing> Listings { get; set; } = [];
}