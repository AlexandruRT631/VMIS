using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using user_backend.DTOs;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController(IMessageService messageService, IMapper mapper) : ControllerBase
{
    [HttpGet("{conversationId}")]
    public IActionResult GetMessages(int conversationId, int pageIndex, int pageSize)
    {
        try
        {
            var messages = messageService.GetMessages(conversationId, pageIndex, pageSize);
            var messageDtos = messages.Select(mapper.Map<MessageDto>).ToList();
            return Ok(messageDtos);
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
    
    [HttpPost]
    public IActionResult AddMessage(MessageDto messageDto, int? listingId)
    {
        try
        {
            var message = mapper.Map<Message>(messageDto);
            var addedMessage = messageService.AddMessage(message, listingId);
            var addedMessageDto = mapper.Map<MessageDto>(addedMessage);
            return Ok(addedMessageDto);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ObjectAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
        catch (EmailException e)
        {
            return Problem(e.Message);
        }
    }
}