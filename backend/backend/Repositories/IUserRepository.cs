using backend.Entities;

namespace backend.Repositories;

public interface IUserRepository
{
    public List<User> GetAllUsers();
    public User? GetUserById(int id);
    public User CreateUser(User user);
    public User UpdateUser(User user);
    public bool DeleteUser(User user);
    public bool DoesUserExist(int id);
}