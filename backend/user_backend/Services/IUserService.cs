using user_backend.Entities;

namespace user_backend.Services;

public interface IUserService
{
    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>List of users</returns>
    public List<User> GetAllUsers();
    
    /// <summary>
    /// Get user with id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>User</returns>
    public User? GetUserById(int id);
    
    /// <summary>
    /// Creates a new user
    /// </summary>
    /// <param name="user"></param>
    /// <returns>User created</returns>
    public User CreateUser(User user);
    
    /// <summary>
    /// Updates a user by the id
    /// </summary>
    /// <param name="user"></param>
    /// <returns>User updated</returns>
    public User UpdateUser(User user);
    
    /// <summary>
    /// Deletes a user by id
    /// </summary>
    /// <param name="user"></param>
    /// <returns>True if the user was deleted</returns>
    public bool DeleteUser(int id);
}