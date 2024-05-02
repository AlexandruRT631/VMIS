using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Category
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)] 
    public string Name { get; set; } = null!;
    public List<Car> PossibleCars { get; set; } = [];
    public List<Listing> Listings { get; set; } = [];
}