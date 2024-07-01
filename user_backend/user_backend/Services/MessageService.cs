using System.Net.WebSockets;
using System.Text.Json;
using user_backend.Constants;
using user_backend.DTOs;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Repositories;
using WebSocketManager = user_backend.WebSockets.WebSocketManager;

namespace user_backend.Services;

public class MessageService(
    IMessageRepository messageRepository,
    IConversationRepository conversationRepository,
    IUserRepository userRepository,
    IEmailService emailService,
    IHttpClientFactory httpClientFactory,
    WebSocketManager webSocketManager
) : IMessageService
{
    public List<Message> GetMessages(int conversationId, int pageIndex, int pageSize)
    {
        if (conversationId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidConversation);
        }
        if (!conversationRepository.DoesConversationExist(conversationId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ConversationNotFound);
        }
        if (pageIndex <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageIndex);
        }
        if (pageSize <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidPageSize);
        }
        
        return messageRepository.GetMessages(conversationId, pageIndex, pageSize);
    }

    public Message AddMessage(Message message, int? listingId = null)
    {
        if (message == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMessage);
        }
        if (message.SenderId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidSender);
        }
        if (!userRepository.DoesUserExist(message.SenderId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.SenderNotFound);
        }
        if (string.IsNullOrWhiteSpace(message.Content))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredContent);
        }
        
        Conversation? conversation;
        Message? addedMessage;
        if (message.Conversation == null)
        {
            if (listingId == null)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidListing);
            }
            if (listingId <= 0)
            {
                throw new InvalidArgumentException(ExceptionMessages.InvalidListing);
            }
            var httpClient = httpClientFactory.CreateClient("listing_backend");
            ListingDto? listingDto;
            try
            {
                listingDto = httpClient.GetFromJsonAsync<ListingDto>($"/api/Listing/{listingId}")
                    .GetAwaiter().GetResult();
            }
            catch (Exception)
            {
                throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
            }
            if (listingDto == null)
            {
                throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
            }
            if (conversationRepository.DoesConversationExist(message.SenderId, listingDto.SellerId, listingId.Value))
            {
                throw new ObjectAlreadyExistsException(ExceptionMessages.ConversationAlreadyExists);
            }
            
            var user1 = userRepository.GetUserById(message.SenderId);
            var user2 = userRepository.GetUserById(listingDto.SellerId);
            conversation = conversationRepository.AddConversation(new Conversation
            {
                ListingId = listingId.Value,
                Users = [user1!, user2!],
                Messages = []
            });
            addedMessage = messageRepository.AddMessage(new Message
            {
                Conversation = conversation,
                SenderId = message.SenderId,
                Content = message.Content,
                SentAt = DateTime.Now
            });
            emailService.SendEmailNewConversation(user2!.Id);
            return addedMessage;
        }
        
        if (message.Conversation.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidConversation);
        }
        if (!conversationRepository.DoesConversationExist(message.Conversation.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.ConversationNotFound);
        }
        
        conversation = conversationRepository.GetConversation(message.Conversation.Id);
        if (conversation!.Users.All(u => u.Id != message.SenderId))
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidSender);
        }
        var lastMessage = conversation.Messages.MaxBy(m => m.SentAt);
        addedMessage = messageRepository.AddMessage(new Message
        {
            Conversation = conversation!,
            SenderId = message.SenderId,
            Content = message.Content,
            SentAt = DateTime.Now
        });
        
        var messageJson = JsonSerializer.Serialize(new
        {
            addedMessage.Id,
            addedMessage.SenderId,
            addedMessage.Content,
            addedMessage.SentAt
        });

        var buffer = new ArraySegment<byte>(System.Text.Encoding.UTF8.GetBytes(messageJson));

        foreach (var socket in webSocketManager.GetSockets(addedMessage.Conversation!.Id.ToString()))
        {
            if (socket.State == WebSocketState.Open)
            {
                socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).GetAwaiter().GetResult();
            }
        }
        
        if (addedMessage.SentAt - lastMessage!.SentAt > TimeSpan.FromMinutes(15))
        {
            emailService.SendEmailNewMessage(conversation!.Users.First(u => u.Id != message.SenderId).Id);
        }
        return addedMessage;
    }
}