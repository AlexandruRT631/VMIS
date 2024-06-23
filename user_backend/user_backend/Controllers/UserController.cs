using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using user_backend.DTOs;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IMapper mapper) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult GetAllUsers()
    {
        return Ok(userService.GetAllUsers());
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
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

    [HttpGet("details/{id}")]
    public IActionResult GetUserDetails(int id)
    {
        try
        {
            return Ok(mapper.Map<UserDto>(userService.GetUserById(id)));
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
    [Authorize(Roles = "Admin")]
    public IActionResult CreateUser([FromForm] string user, [FromForm] IFormFile? profileImage)
    {
        try
        {
            var userObject = JsonSerializer.Deserialize<User>(user);
            return Ok(userService.CreateUser(userObject, profileImage));
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
    [Authorize(Roles = "Admin")]
    public IActionResult UpdateUser([FromForm] string user, [FromForm] IFormFile? profileImage)
    {
        try
        {
            var userObject = JsonSerializer.Deserialize<User>(user);
            return Ok(userService.UpdateUser(userObject, profileImage));
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
    [Authorize(Roles = "Admin")]
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