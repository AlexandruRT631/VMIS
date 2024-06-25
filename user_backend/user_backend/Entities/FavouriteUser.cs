using System.ComponentModel.DataAnnotations;

namespace user_backend.Entities;

public class FavouriteUser
{
    [Key]
    public int Id { get; set; }
    public int FavouriteUserId { get; set; }
    public virtual User User { get; set; } = null!;
}