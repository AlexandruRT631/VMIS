using System.ComponentModel.DataAnnotations;

namespace listing_backend.Entities;

public class Make
{
   [Key]
   public int Id { get; set; }
   [MaxLength(100)] 
   public string Name { get; set; } = null!;
   public virtual List<Model> PossibleModels { get; set; } = [];
   public virtual List<Engine> PossibleEngines { get; set; } = [];
}