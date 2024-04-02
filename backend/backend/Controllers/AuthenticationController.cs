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
            var accessToken = _authenticationService.AuthenticateUser(userLoginDto.Email, userLoginDto.Password);
            var refreshToken = _authenticationService.GenerateRefreshToken(userLoginDto.Email);
            return Ok(new TokenDto(accessToken, refreshToken));
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
            var accessToken = _authenticationService.RegisterUser(user);
            var refreshToken = _authenticationService.GenerateRefreshToken(user.Email!);
            return Ok(new TokenDto(accessToken, refreshToken));
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
    
    [HttpPost("refresh")]
    public IActionResult Refresh(string refreshToken)
    {
        try
        {
            var tokenDto = _authenticationService.IsRefreshTokenValid(refreshToken);
            return Ok(tokenDto);
        }
        catch (InvalidArgumentException e)
        {
            return BadRequest(e.Message);
        }
        catch (ExpiredException e)
        {
            return Unauthorized(e.Message);
        }
    }
}