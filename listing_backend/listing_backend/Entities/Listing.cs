using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Listing
{
    [Key]
    public int Id { get; set; }
    public int SellerId { get; set; }
    public int Price { get; set; }
    public virtual Car? Car { get; set; }
    public int Year { get; set; }
    public virtual Category? Category { get; set; }
    public virtual Engine? Engine { get; set; }
    public virtual DoorType? DoorType { get; set; }
    public virtual Transmission? Transmission { get; set; }
    public virtual Traction? Traction { get; set; }
    public virtual Color? InteriorColor { get; set; }
    public virtual Color? ExteriorColor { get; set; }
    public int Mileage { get; set; }
    public virtual List<FeatureExterior>? FeaturesExterior { get; set; } = [];
    public virtual List<FeatureInterior>? FeaturesInterior { get; set; } = [];
}