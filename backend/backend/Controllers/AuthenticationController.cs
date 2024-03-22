using backend.DTOs;
using backend.Entities;
using backend.Exceptions;
using backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;
    
    public AuthenticationController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }
    
    [HttpPost("login")]
    public IActionResult Login(UserLoginDto userLoginDto)
    {
        try
        {
            var token = _authenticationService.AuthenticateUser(userLoginDto.Email, userLoginDto.Password);
            return Ok(new { Token = token });
        }
        catch (InvalidCredentialsException e)
        {
            return NotFound(e.Message);
        }
    }
    
    [HttpPost("register")]
    public IActionResult Register(User user)
    {
        try
        {
            var token = _authenticationService.RegisterUser(user);
            return Ok(new { Token = token });
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
}