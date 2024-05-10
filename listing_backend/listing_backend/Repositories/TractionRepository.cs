using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class TractionRepository(ListingDbContext context) : ITractionRepository
{
    public List<Traction> GetAllTractions()
    {
        return context.Tractions
            .ToList();
    }

    public Traction? GetTractionById(int id)
    {
        return context.Tractions
            .FirstOrDefault(e => e.Id == id);
    }
    
    public Traction CreateTraction(Traction traction)
    {
        context.Tractions.Add(traction);
        context.SaveChanges();
        return traction;
    }
    
    public Traction UpdateTraction(Traction traction)
    {
        context.Tractions.Update(traction);
        context.SaveChanges();
        return traction;
    }
    
    public bool DeleteTraction(Traction traction)
    {
        context.Tractions.Remove(traction);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesTractionExist(int id)
    {
        return context.Tractions.Any(e => e.Id == id);
    }
    
    public bool DoesTractionExist(string name)
    {
        return context.Tractions.Any(e => e.Name == name);
    }
}