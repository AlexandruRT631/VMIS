using backend.DTOs;
using backend.Entities;

namespace backend.Services;

public interface IAuthenticationService
{
    /// <summary>
    /// Try to authenticate a user
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns>Jwt Token</returns>
    public string AuthenticateUser(string email, string password);
    
    /// <summary>
    /// Register a user and authenticate them
    /// </summary>
    /// <param name="user"></param>
    /// <returns>Jwt Token</returns>
    public string RegisterUser(User user);
    
    /// <summary>
    /// Generates a refresh token, and stores it in the user database
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Refresh Token</returns>
    public string GenerateRefreshToken(string email);
    
    /// <summary>
    /// Checks if a refresh token is valid and not expired
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>New Access and Refresh Tokens</returns>
    public TokenDto IsRefreshTokenValid(string refreshToken);

    /// <summary>
    /// Check if a token is valid
    /// </summary>
    /// <param name="token"></param>
    /// <returns>Boolean</returns>
    public bool IsTokenValid(string token);
}