using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class FeatureInterior
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public virtual List<Listing> Listings { get; set; } = [];
}