using backend.Entities;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public IActionResult GetAllUsers()
    {
        return Ok(_userService.GetAllUsers());
    }

    [HttpGet("{id}")]
    public IActionResult GetUserById(int id)
    {
        return Ok(_userService.GetUserById(id));
    }

    [HttpPost]
    public IActionResult CreateUser(User user)
    {
        return Ok(_userService.CreateUser(user));
    }

    [HttpPut]
    public IActionResult UpdateUser(User user)
    {
        return Ok(_userService.UpdateUser(user));
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteUser(int id)
    {
        return Ok(_userService.DeleteUser(id));
    }
}