using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class FuelService(IFuelRepository fuelRepository) : IFuelService
{
    public List<Fuel> GetAllFuels()
    {
        return fuelRepository.GetAllFuels();
    }

    public Fuel? GetFuelById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!fuelRepository.DoesFuelExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FuelNotFound);
        }
        
        return fuelRepository.GetFuelById(id);
    }

    public Fuel CreateFuel(Fuel fuel)
    {
        if (fuel == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFuel);
        }
        if (fuel.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (fuelRepository.DoesFuelExist(fuel.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FuelAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(fuel.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (fuelRepository.DoesFuelExist(fuel.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FuelAlreadyExists);
        }
        
        return fuelRepository.CreateFuel(fuel);
    }

    public Fuel UpdateFuel(Fuel fuel)
    {
        
        if (fuel == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFuel);
        }
        if (fuel.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!fuelRepository.DoesFuelExist(fuel.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FuelNotFound);
        }
        if (string.IsNullOrWhiteSpace(fuel.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (fuelRepository.DoesFuelExist(fuel.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FuelAlreadyExists);
        }
        
        return fuelRepository.UpdateFuel(fuel);
    }

    public bool DeleteFuel(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!fuelRepository.DoesFuelExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FuelNotFound);
        }
        
        var fuel = fuelRepository.GetFuelById(id);
        return fuelRepository.DeleteFuel(fuel!);
    }
}