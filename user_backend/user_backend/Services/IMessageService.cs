using user_backend.Entities;

namespace user_backend.Services;

public interface IMessageService
{
    public List<Message> GetMessages(int conversationId, int pageIndex, int pageSize);
    public Message AddMessage(Message message, int? listingId);
}