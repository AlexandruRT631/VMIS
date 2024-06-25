using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using user_backend.Constants;
using user_backend.DTOs;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Repositories;

namespace user_backend.Services;

public class AuthenticationService(
    IUserRepository userRepository, 
    IImageService imageService,
    IConfiguration configuration
) : IAuthenticationService
{
    public string AuthenticateUser(string email, string password)
    {
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            throw new InvalidCredentialsException(ExceptionMessages.InvalidCredentials);
        }
        var user = userRepository.GetUserByEmail(email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            throw new InvalidCredentialsException(ExceptionMessages.InvalidCredentials);
        }

        return GenerateJwtToken(user);
    }
    
    public string RegisterUser(User? user, IFormFile? profileImage)
    {
        if (user == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredEmail);
        }
        if (userRepository.DoesUserExist(user.Email))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.UsedEmail);
        }
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPassword);
        }
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (user.Role != UserRole.Client && user.Role != UserRole.Seller)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUserRole);
        }
        if (user.RefreshToken != null)
        {
            throw new InvalidArgumentException(ExceptionMessages.ExcludedRefreshToken);
        }
        
        user.Id = 0;
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        user.ProfilePictureUrl = profileImage == null
            ? imageService.GetDefaultImageUrl()
            : imageService.SaveImage(profileImage);
        user = userRepository.CreateUser(user);
        return GenerateJwtToken(user);
    }

    public string GenerateRefreshToken(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredEmail);
        }
        var user = userRepository.GetUserByEmail(email);
        if (user == null)
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        
        var token = GenerateRandomToken();
        user.RefreshToken = token;
        user.RefreshTokenExpiration = DateTime.UtcNow.AddDays(7);
        userRepository.UpdateUser(user);
        return token;
    }
    
    public TokenDto IsRefreshTokenValid(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredRefreshToken);
        }
        var user = userRepository.GetUserByRefreshToken(refreshToken);
        if (user == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidRefreshToken);
        }
        if (user.RefreshTokenExpiration < DateTime.UtcNow)
        {
            user.RefreshToken = null;
            userRepository.UpdateUser(user);
            throw new ExpiredException(ExceptionMessages.ExpiredRefreshToken);
        }
        
        var newRefreshToken = GenerateRandomToken();
        user.RefreshToken = newRefreshToken;
        userRepository.UpdateUser(user);
        var accessToken = GenerateJwtToken(user);
        return new TokenDto(accessToken, newRefreshToken);
    }

    public bool IsTokenValid(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name!),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim("ProfilePictureUrl", user.ProfilePictureUrl!)
            }),
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRandomToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}