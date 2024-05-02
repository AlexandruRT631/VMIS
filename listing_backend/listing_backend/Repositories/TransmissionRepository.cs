using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class TransmissionRepository(ListingDbContext context) : ITransmissionRepository
{
    public List<Transmission> GetAllTransmissions()
    {
        return context.Transmissions.ToList();
    }
    
    public Transmission? GetTransmissionById(int id)
    {
        return context.Transmissions.Find(id);
    }

    public Transmission CreateTransmission(Transmission transmission)
    {
        context.Transmissions.Add(transmission);
        context.SaveChanges();
        return transmission;
    }
    
    public Transmission UpdateTransmission(Transmission transmission)
    {
        context.Transmissions.Update(transmission);
        context.SaveChanges();
        return transmission;
    }
    
    public bool DeleteTransmission(Transmission transmission)
    {
        context.Transmissions.Remove(transmission);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesTransmissionExist(int id)
    {
        return context.Transmissions.Any(e => e.Id == id);
    }
}