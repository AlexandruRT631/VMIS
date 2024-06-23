using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Listing
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int SellerId { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }
    public virtual Car? Car { get; set; }
    public int Year { get; set; }
    public virtual Category? Category { get; set; }
    public virtual Engine? Engine { get; set; }
    public virtual DoorType? DoorType { get; set; }
    public virtual Transmission? Transmission { get; set; }
    public virtual Traction? Traction { get; set; }
    public virtual Color? Color { get; set; }
    public int Mileage { get; set; }
    public virtual List<Feature>? Features { get; set; } = [];
    public virtual List<ListingImage>? ListingImages { get; set; } = [];
    public DateTime CreatedAt { get; set; }
    public bool? IsSold { get; set; }
}