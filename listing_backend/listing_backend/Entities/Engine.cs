using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Engine
{
    [Key]
    public int Id { get; set; }
    public virtual Make? Make { get; set; }
    public string EngineCode { get; set; } = null!;
    public int Displacement { get; set; }
    public virtual Fuel? Fuel { get; set; }
    public int Power { get; set; }
    public int Torque { get; set; }
    public virtual List<Car> PossibleCars { get; set; } = [];
    public virtual List<Listing> Listings { get; set; } = [];
}