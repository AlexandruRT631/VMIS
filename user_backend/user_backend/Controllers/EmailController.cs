using Microsoft.AspNetCore.Mvc;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmailController(IEmailService emailService) : ControllerBase
{
    [HttpPost("sendUpdateListing/{id:int}")]
    public IActionResult SendEmailUpdateListing(int id)
    {
        try
        {
            return Ok(emailService.SendEmailUpdateListing(id));
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (EmailException e)
        {
            return Problem(e.Message);
        }
    }
    
    [HttpPost("sendNewListing/{sellerId:int}/{listingId:int}")]
    public IActionResult SendEmailNewListing(int sellerId, int listingId)
    {
        try
        {
            return Ok(emailService.SendEmailNewListing(sellerId, listingId));
        }
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (EmailException e)
        {
            return Problem(e.Message);
        }
    }
}