using backend.Entities;
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
        return _userRepository.GetUserById(id);
    }
    
    public User CreateUser(User user)
    {
        return _userRepository.CreateUser(user);
    }
    
    public User UpdateUser(User user)
    {
        return _userRepository.UpdateUser(user);
    }
    
    public bool DeleteUser(int id)
    {
        if (_userRepository.DoesUserExist(id))
        {
            var user = _userRepository.GetUserById(id);
            return _userRepository.DeleteUser(user);
        }
        return false;
    }
}