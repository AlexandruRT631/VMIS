using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class ListingImage
{
    [Key]
    public int Id { get; set; }
    public string? Url { get; set; }
    public virtual Listing Listing { get; set; } = null!;
}