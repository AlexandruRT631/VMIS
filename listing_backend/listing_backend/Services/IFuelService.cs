using listing_backend.Entities;

namespace listing_backend.Services;

public interface IFuelService
{
    public List<Fuel> GetAllFuels();
    public Fuel? GetFuelById(int id);
    public Fuel CreateFuel(Fuel fuel);
    public Fuel UpdateFuel(Fuel fuel);
    public bool DeleteFuel(int id);
}