using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class FuelRepository(ListingDbContext context) : IFuelRepository
{
    public List<Fuel> GetAllFuels()
    {
        return context.Fuels.ToList();
    }

    public Fuel? GetFuelById(int id)
    {
        return context.Fuels.Find(id);
    }
    
    public Fuel CreateFuel(Fuel fuel)
    {
        context.Fuels.Add(fuel);
        context.SaveChanges();
        return fuel;
    }
    
    public Fuel UpdateFuel(Fuel fuel)
    {
        context.Fuels.Update(fuel);
        context.SaveChanges();
        return fuel;
    }
    
    public bool DeleteFuel(Fuel fuel)
    {
        context.Fuels.Remove(fuel);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesFuelExist(int id)
    {
        return context.Fuels.Any(e => e.Id == id);
    }
    
    public bool DoesFuelExist(string name)
    {
        return context.Fuels.Any(e => e.Name == name);
    }
}