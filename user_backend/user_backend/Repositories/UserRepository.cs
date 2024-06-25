using user_backend.DataAccess;
using user_backend.Entities;
using user_backend.Utils;

namespace user_backend.Repositories;

public class UserRepository(UserDbContext context) : IUserRepository
{
    public List<User> GetAllUsers()
    {
        return context.Users.ToList();
    }
    
    public User? GetUserById(int id)
    {
        return context.Users.Find(id);
    }
    
    public User? GetUserByEmail(string email)
    {
        return context.Users.FirstOrDefault(u => u.Email == email);
    }
    
    public User? GetUserByRefreshToken(string refreshToken)
    {
        return context.Users.FirstOrDefault(u => u.RefreshToken == refreshToken);
    }
    
    public User CreateUser(User user)
    {
        context.Users.Add(user);
        context.SaveChanges();
        return user;
    }
    
    public User UpdateUser(User user)
    {
        context.Users.Update(user);
        context.SaveChanges();
        return user;
    }
    
    public bool DeleteUser(User user)
    {
        context.Users.Remove(user);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesUserExist(int id)
    {
        return context.Users.Any(u => u.Id == id);
    }
    
    public bool DoesUserExist(string email)
    {
        return context.Users.Any(u => u.Email == email);
    }

    public (List<User>, int) GetUsersByIds(List<int> ids, int pageIndex, int pageSize)
    {
        var orderedUsers = context.Users
            .Where(u => ids.Contains(u.Id))
            .OrderBy(u => u.Id);
        var totalPages = (int)Math.Ceiling(orderedUsers.Count() / (double)pageSize);
        var users = QueryUtilities.Paginate(orderedUsers, pageIndex, pageSize);
        return (users.ToList(), totalPages);
    }
}