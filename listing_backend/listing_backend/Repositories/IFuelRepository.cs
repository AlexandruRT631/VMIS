using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IFuelRepository
{
    public List<Fuel> GetAllFuels();
    public Fuel? GetFuelById(int id);
    public Fuel CreateFuel(Fuel fuel);
    public Fuel UpdateFuel(Fuel fuel);
    public bool DeleteFuel(Fuel fuel);
    public bool DoesFuelExist(int id);
}