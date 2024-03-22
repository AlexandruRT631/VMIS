using backend.DataAccess;
using backend.Entities;

namespace backend.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BackendDbContext _context;
    
    public UserRepository(BackendDbContext context)
    {
        _context = context;
    }
    
    public List<User> GetAllUsers()
    {
        return _context.Users.ToList();
    }
    
    public User? GetUserById(int id)
    {
        return _context.Users.Find(id);
    }
    
    public User? GetUserByEmail(string email)
    {
        return _context.Users.FirstOrDefault(u => u.Email == email);
    }
    
    public User CreateUser(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
        return user;
    }
    
    public User UpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return user;
    }
    
    public bool DeleteUser(User user)
    {
        _context.Users.Remove(user);
        _context.SaveChanges();
        return true;
    }
    
    public bool DoesUserExist(int id)
    {
        return _context.Users.Any(u => u.Id == id);
    }
    
    public bool DoesUserExist(string email)
    {
        return _context.Users.Any(u => u.Email == email);
    }
}