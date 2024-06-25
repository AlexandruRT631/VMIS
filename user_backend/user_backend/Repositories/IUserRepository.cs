using user_backend.Entities;

namespace user_backend.Repositories;

public interface IUserRepository
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
    /// Get user with email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>User</returns>
    public User? GetUserByEmail(string email);
    
    /// <summary>
    /// Get user with refresh token
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>User</returns>
    public User? GetUserByRefreshToken(string refreshToken);
    
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
    public bool DeleteUser(User user);
    
    /// <summary>
    /// Checks if a user exists by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Boolean</returns>
    public bool DoesUserExist(int id);
    
    /// <summary>
    /// Checks if a user exists by email
    /// </summary>
    /// <param name="email"></param>
    /// <returns>Boolean</returns>
    public bool DoesUserExist(string email);
    public (List<User>, int) GetUsersByIds(List<int> ids, int pageIndex, int pageSize);
}