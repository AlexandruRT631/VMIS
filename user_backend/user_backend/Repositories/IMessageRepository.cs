using user_backend.Entities;

namespace user_backend.Repositories;

public interface IMessageRepository
{
    public List<Message> GetMessages(int conversationId, int pageIndex, int pageSize);
    public Message AddMessage(Message message);
}