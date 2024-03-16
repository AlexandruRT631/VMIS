using backend.Entities;

namespace backend.Services;

public interface IUserService
{
    public List<User> GetAllUsers();
    public User? GetUserById(int id);
    public User CreateUser(User user);
    public User UpdateUser(User user);
    public bool DeleteUser(int id);
}