using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Color
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public virtual List<Listing> ListingsInterior { get; set; } = [];
    public virtual List<Listing> ListingsExterior { get; set; } = [];
}