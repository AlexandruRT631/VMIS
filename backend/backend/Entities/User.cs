using System.ComponentModel.DataAnnotations;

namespace backend.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Name { get; set; }
    public UserRole Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiration { get; set; }
}