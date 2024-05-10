using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Listing
{
    [Key]
    public int Id { get; set; }
    public int SellerId { get; set; }
    public int Price { get; set; }
    public virtual Car Car { get; set; } = null!;
    public int Year { get; set; }
    public virtual Category Category { get; set; } = null!;
    public virtual Engine Engine { get; set; } = null!;
    public virtual DoorType DoorType { get; set; } = null!;
    public virtual Color InteriorColor { get; set; } = null!;
    public virtual Color ExteriorColor { get; set; } = null!;
    public int Mileage { get; set; }
    public virtual List<FeatureExterior> FeaturesExterior { get; set; } = [];
    public virtual List<FeatureInterior> FeaturesInterior { get; set; } = [];
}