namespace user_backend.DTOs;

public class MessageDto
{
    public int? Id { get; set; }
    public ConversationDto? Conversation { get; set; }
    public int? SenderId { get; set; }
    public string? Content { get; set; }
    public DateTime? SentAt { get; set; }
}