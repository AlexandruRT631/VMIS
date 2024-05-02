using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class MakeRepository(ListingDbContext context) : IMakeRepository
{
    public List<Make> GetAllMakes()
    {
        return context.Makes.ToList();
    }

    public Make? GetMakeById(int id)
    {
        return context.Makes.Find(id);
    }
    
    public Make CreateMake(Make make)
    {
        context.Makes.Add(make);
        context.SaveChanges();
        return make;
    }
    
    public Make UpdateMake(Make make)
    {
        context.Makes.Update(make);
        context.SaveChanges();
        return make;
    }
    
    public bool DeleteMake(Make make)
    {
        context.Makes.Remove(make);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesMakeExist(int id)
    {
        return context.Makes.Any(e => e.Id == id);
    }
}