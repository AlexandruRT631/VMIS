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
    /// Check if a token is valid
    /// </summary>
    /// <param name="token"></param>
    /// <returns>Boolean</returns>
    public bool IsTokenValid(string token);
}