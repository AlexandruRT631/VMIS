using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(userService.GetAllUsers());
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        try
        {
            return Ok(userService.GetUserById(id));
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        try
        {
            return Ok(userService.CreateUser(user));
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut]
    public IActionResult UpdateUser(User user)
    {
        try
        {
            return Ok(userService.UpdateUser(user));
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (UserAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        try
        {
            return Ok(userService.DeleteUser(id));
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (UserNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}