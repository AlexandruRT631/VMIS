using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Listing
{
    [Key]
    public int Id { get; set; }
    public int SellerId { get; set; }
    public int Price { get; set; }
    public Car Car { get; set; } = null!;
    public int Year { get; set; }
    public Category Category { get; set; } = null!;
    public Engine Engine { get; set; } = null!;
    public DoorType DoorType { get; set; } = null!;
    public Color InteriorColor { get; set; } = null!;
    public Color ExteriorColor { get; set; } = null!;
    public int Mileage { get; set; }
    public List<FeatureExterior> FeaturesExterior { get; set; } = [];
    public List<FeatureInterior> FeaturesInterior { get; set; } = [];
}