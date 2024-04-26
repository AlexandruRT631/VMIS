namespace user_backend.DTOs;

public class UserLoginDto(string email, string password)
{
    public string Email { get; } = email;
    public string Password { get; } = password;
}