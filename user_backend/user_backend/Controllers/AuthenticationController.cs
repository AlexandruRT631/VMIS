using Microsoft.AspNetCore.Mvc;
using user_backend.DTOs;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Services;

namespace user_backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController(IAuthenticationService authenticationService) : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login(UserLoginDto userLoginDto)
    {
        try
        {
            var accessToken = authenticationService.AuthenticateUser(userLoginDto.Email, userLoginDto.Password);
            var refreshToken = authenticationService.GenerateRefreshToken(userLoginDto.Email);
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
            var accessToken = authenticationService.RegisterUser(user);
            var refreshToken = authenticationService.GenerateRefreshToken(user.Email!);
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
            var tokenDto = authenticationService.IsRefreshTokenValid(refreshToken);
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