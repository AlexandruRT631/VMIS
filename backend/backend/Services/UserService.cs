using backend.Constants;
using backend.Entities;
using backend.Exceptions;
using backend.Repositories;

namespace backend.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    
    public List<User> GetAllUsers()
    {
        return _userRepository.GetAllUsers();
    }

    public User? GetUserById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!_userRepository.DoesUserExist(id))
        {
            throw new UserNotFoundException(ExceptionMessages.UserNotFound);
        }
        
        return _userRepository.GetUserById(id);
    }
    
    public User CreateUser(User user)
    {
        if (user == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (user.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (user.Id != 0 && _userRepository.DoesUserExist(user.Id))
        {
            throw new UserAlreadyExistsException(ExceptionMessages.UserAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredEmail);
        }
        if (_userRepository.DoesUserExist(user.Email))
        {
            throw new UserAlreadyExistsException(ExceptionMessages.UsedEmail);
        }
        if (string.IsNullOrWhiteSpace(user.Password))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredPassword);
        }
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (!Enum.IsDefined(typeof(UserRole), user.Role) || user.Role == UserRole.None) 
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUserRole);
        }
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        return _userRepository.CreateUser(user);
    }
    
    public User UpdateUser(User user)
    {
        if (user == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (user.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!_userRepository.DoesUserExist(user.Id))
        {
            throw new UserNotFoundException(ExceptionMessages.UserNotFound);
        }
        if (!string.IsNullOrWhiteSpace(user.Email) && _userRepository.DoesUserExist(user.Email))
        {
            throw new UserAlreadyExistsException(ExceptionMessages.UsedEmail);
        }
        if (!Enum.IsDefined(typeof(UserRole), user.Role)) 
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUserRole);
        }
        
        var existingUser = _userRepository.GetUserById(user.Id);
        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            existingUser!.Email = user.Email;
        }
        if (!string.IsNullOrWhiteSpace(user.Password))
        {
            existingUser!.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
        }
        if (!string.IsNullOrWhiteSpace(user.Name))
        {
            existingUser!.Name = user.Name;
        }
        if (user.Role != UserRole.None)
        {
            existingUser!.Role = user.Role;
        }
        return _userRepository.UpdateUser(existingUser!);
    }
    
    public bool DeleteUser(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!_userRepository.DoesUserExist(id))
        {
            throw new UserNotFoundException(ExceptionMessages.UserNotFound);
        }
        
        var user = _userRepository.GetUserById(id);
        return _userRepository.DeleteUser(user!);
    }
}