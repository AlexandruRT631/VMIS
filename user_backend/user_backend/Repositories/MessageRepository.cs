using user_backend.DataAccess;
using user_backend.Entities;
using user_backend.Utils;

namespace user_backend.Repositories;

public class MessageRepository(UserDbContext context) : IMessageRepository
{
    public List<Message> GetMessages(int conversationId, int pageIndex, int pageSize)
    {
        var orderedMessages = context.Messages
            .Where(m => m.Conversation.Id == conversationId)
            .OrderBy(m => m.SentAt);
        var messages = QueryUtilities.Paginate(orderedMessages, pageIndex, pageSize);
        return messages.ToList();
    }

    public Message AddMessage(Message message)
    {
        context.Messages.Add(message);
        context.SaveChanges();
        return message;
    }
}