using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Model
{
    [Key]
    public int Id { get; set; }
    public virtual Make? Make { get; set; }
    [MaxLength(100)]
    public string? Name { get; set; }
    public virtual List<Car> PossibleCars { get; set; } = [];
}