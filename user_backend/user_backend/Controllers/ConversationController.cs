using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using user_backend.DTOs;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConversationController(IConversationService conversationService, IMapper mapper) : ControllerBase
{
    [HttpGet("{userId}")]
    public IActionResult GetConversations(int userId, int pageIndex, int pageSize)
    {
        try
        {
            var (conversations, totalPages) = conversationService.GetConversations(userId, pageIndex, pageSize);
            var conversationDtos = conversations.Select(mapper.Map<ConversationDto>).ToList();
            return Ok(new { conversations = conversationDtos, totalPages });
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}