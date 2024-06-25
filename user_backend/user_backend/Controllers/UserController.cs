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
        var userDtos = userService.GetAllUsers().Select(mapper.Map<UserDto>).ToList();
        return Ok(userDtos);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public IActionResult GetUserById(int id)
    {
        try
        {
            var userDto = mapper.Map<UserDto>(userService.GetUserById(id));
            return Ok(userDto);
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

    [HttpGet("details/{id}")]
    public IActionResult GetUserDetails(int id)
    {
        try
        {
            var userDetailsDto = mapper.Map<UserDetailsDto>(userService.GetUserById(id));
            return Ok(userDetailsDto);
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
    [Authorize(Roles = "Admin")]
    public IActionResult CreateUser([FromForm] string user, [FromForm] IFormFile? profileImage)
    {
        try
        {
            var userDto = JsonSerializer.Deserialize<UserDto>(user);
            var userObject = mapper.Map<User>(userDto);
            var userCreated = userService.CreateUser(userObject, profileImage);
            return Ok(mapper.Map<UserDto>(userCreated));
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ObjectAlreadyExistsException e)
        {
            return Conflict(e.Message);
        }
    }

    [HttpPut]
    public IActionResult UpdateUser([FromForm] string user, [FromForm] IFormFile? profileImage)
    {
        try
        {
            var userDto = JsonSerializer.Deserialize<UserDto>(user);
            var userObject = mapper.Map<User>(userDto);
            var userUpdated = userService.UpdateUser(userObject, profileImage);
            return Ok(mapper.Map<UserDto>(userUpdated));
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
        catch (ObjectNotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("details/ids")]
    public IActionResult GetUsersDetailByIds(List<int> ids, int pageIndex, int pageSize)
    {
        try
        {
            var (users, totalPages) = userService
                .GetUsersByIds(ids, pageIndex, pageSize);
            var userDetailDtos = users
                .Select(mapper.Map<UserDetailsDto>)
                .ToList();
            return Ok(new
            {
                Users = userDetailDtos,
                TotalPages = totalPages
            });
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