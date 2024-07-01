using user_backend.Constants;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Repositories;

namespace user_backend.Services;

public class ConversationService(
    IConversationRepository conversationRepository,
    IUserRepository userRepository
) : IConversationService
{
    public (List<Conversation>, int) GetConversations(int userId, int pageIndex, int pageSize)
    {
        if (userId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (!userRepository.DoesUserExist(userId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        if (pageIndex <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageIndex);
        }
        if (pageSize <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageSize);
        }
        
        return conversationRepository.GetConversations(userId, pageIndex, pageSize);
    }
}