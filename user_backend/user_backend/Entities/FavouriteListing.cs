using System.ComponentModel.DataAnnotations;

namespace user_backend.Entities;

public class FavouriteListing
{
    [Key]
    public int Id { get; set; }
    public int FavouriteListingId { get; set; }
    public virtual User User { get; set; } = null!;
}