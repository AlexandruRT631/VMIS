using System.ComponentModel.DataAnnotations;

namespace user_backend.Entities;

public class Message
{
    [Key]
    public int Id { get; set; }
    public virtual Conversation? Conversation { get; set; }
    public int SenderId { get; set; }
    public string? Content { get; set; }
    public DateTime SentAt { get; set; }
}