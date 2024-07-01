using user_backend.DataAccess;
using user_backend.Entities;
using user_backend.Utils;

namespace user_backend.Repositories;

public class ConversationRepository(UserDbContext context) : IConversationRepository
{
    public (List<Conversation>, int) GetConversations(int userId, int pageIndex, int pageSize)
    {
        var orderedConversations = context.Conversations
            .Where(c => c.Users.Any(u => u.Id == userId))
            .OrderByDescending(c => c.Messages.Max(m => m.SentAt));
        var totalPages = (int) Math.Ceiling(orderedConversations.Count() / (double) pageSize);
        var conversations = QueryUtilities.Paginate(orderedConversations, pageIndex, pageSize);
        return (conversations.ToList(), totalPages);
    }

    public Conversation AddConversation(Conversation conversation)
    {
        context.Conversations.Add(conversation);
        context.SaveChanges();
        return conversation;
    }

    public Conversation? GetConversation(int conversationId)
    {
        return context.Conversations.Find(conversationId);
    }

    public bool DoesConversationExist(int conversationId)
    {
        return context.Conversations.Any(c => c.Id == conversationId);
    }

    public bool DoesConversationExist(int user1Id, int user2Id, int listingId)
    {
        return context.Conversations.Any(c =>
            c.Users.Any(u => u.Id == user1Id) &&
            c.Users.Any(u => u.Id == user2Id) &&
            c.ListingId == listingId);
    }
}