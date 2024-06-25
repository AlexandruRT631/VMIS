using System.ComponentModel.DataAnnotations;

namespace user_backend.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    [MaxLength(320)]
    public string? Email { get; set; }
    [MaxLength(512)]
    public string? Password { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    public UserRole Role { get; set; }
    public string? ProfilePictureUrl { get; set; }
    [MaxLength(512)]
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
    public virtual List<FavouriteListing>? FavouriteListings { get; set; } = [];
    public virtual List<FavouriteUser>? FavouriteUsers { get; set; } = [];
}