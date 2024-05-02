using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Model
{
    [Key]
    public int Id { get; set; }
    public Make Make { get; set; } = null!;
    [MaxLength(100)]
    public string Name { get; set; } = null!;
}