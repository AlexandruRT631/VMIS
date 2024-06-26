using Microsoft.AspNetCore.Mvc;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FavouriteController(IFavouriteService favouriteService) : ControllerBase
{
    [HttpPost("addFavouriteListing")]
    public IActionResult AddFavouriteListing(int userId, int favouriteListingId)
    {
        try
        {
            favouriteService.AddFavouriteListing(userId, favouriteListingId);
            return Ok();
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
    }
    
    [HttpDelete("removeFavouriteListing")]
    public IActionResult RemoveFavouriteListing(int userId, int favouriteListingId)
    {
        try
        {
            favouriteService.RemoveFavouriteListing(userId, favouriteListingId);
            return Ok();
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
    
    [HttpPost("addFavouriteUser")]
    public IActionResult AddFavouriteUser(int userId, int favouriteUserId)
    {
        try
        {
            favouriteService.AddFavouriteUser(userId, favouriteUserId);
            return Ok();
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
    }
    
    [HttpDelete("removeFavouriteUser")]
    public IActionResult RemoveFavouriteUser(int userId, int favouriteUserId)
    {
        try
        {
            favouriteService.RemoveFavouriteUser(userId, favouriteUserId);
            return Ok();
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
    
    [HttpDelete("removeFavouriteListings/{favouriteListingId:int}")]
    public IActionResult RemoveListingFromFavourites(int favouriteListingId)
    {
        try
        {
            return Ok(favouriteService.RemoveListingFromFavourites(favouriteListingId));
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
    
    [HttpPost("removeFavouriteListings")]
    public IActionResult RemoveListingsFromFavourites(List<int> favouriteListingIds)
    {
        try
        {
            return Ok(favouriteService.RemoveListingsFromFavourites(favouriteListingIds));
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