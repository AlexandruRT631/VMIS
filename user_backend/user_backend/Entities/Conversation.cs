using System.ComponentModel.DataAnnotations;

namespace user_backend.Entities;

public class Conversation
{
    [Key]
    public int Id { get; set; }
    public int ListingId { get; set; }
    public virtual List<User> Users { get; set; } = [];
    public virtual List<Message> Messages { get; set; } = [];
}