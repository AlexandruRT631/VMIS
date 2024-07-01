using user_backend.Entities;

namespace user_backend.Repositories;

public interface IConversationRepository
{
    public (List<Conversation>, int) GetConversations(int userId, int pageIndex, int pageSize);
    public Conversation AddConversation(Conversation conversation);
    public Conversation? GetConversation(int conversationId);
    public bool DoesConversationExist(int conversationId);
    public bool DoesConversationExist(int user1Id, int user2Id, int listingId);
}