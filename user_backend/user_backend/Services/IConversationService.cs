using user_backend.Entities;

namespace user_backend.Services;

public interface IConversationService
{
    public (List<Conversation>, int) GetConversations(int userId, int pageIndex, int pageSize);
}