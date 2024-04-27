using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Fuel
{
    [Key]
    public int Id { get; set; }
    [MaxLength(100)]
    public string Name { get; set; } = null!;
    public List<Engine> PossibleEngines { get; set; } = [];
}