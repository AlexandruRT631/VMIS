using listing_backend.DataAccess;
using listing_backend.Entities;
using listing_backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace listing_backend.Repositories;

public class CarRepository(ListingDbContext context) : ICarRepository
{
    public List<Car> GetAllCars()
    {
        return context.Cars
            .ToList();
    }

    public Car? GetCarById(int id)
    {
        return context.Cars
            .Include(existingCar => existingCar.PossibleCategories!)
            .Include(existingCar => existingCar.PossibleDoorTypes!)
            .Include(existingCar => existingCar.PossibleTransmissions!)
            .Include(existingCar => existingCar.PossibleTractions!)
            .Include(existingCar => existingCar.PossibleEngines!)
            .FirstOrDefault(c => c.Id == id);
    }

    public Car CreateCar(Car car)
    {
        context.Cars.Add(car);
        context.SaveChanges();
        return car;
    }

    public Car UpdateCar(Car car)
    {
        var existingCar = context.Cars
            .Include(existingCar => existingCar.PossibleCategories!)
            .Include(existingCar => existingCar.PossibleDoorTypes!)
            .Include(existingCar => existingCar.PossibleTransmissions!)
            .Include(existingCar => existingCar.PossibleTractions!)
            .Include(existingCar => existingCar.PossibleEngines!)
            .FirstOrDefault(c => c.Id == car.Id);
        
        context.Entry(existingCar!).CurrentValues.SetValues(car);
        
        Utilities.UpdateCollection(existingCar!.PossibleCategories!, car!.PossibleCategories!);
        Utilities.UpdateCollection(existingCar!.PossibleDoorTypes!, car!.PossibleDoorTypes!);
        Utilities.UpdateCollection(existingCar!.PossibleTransmissions!, car!.PossibleTransmissions!);
        Utilities.UpdateCollection(existingCar!.PossibleTractions!, car!.PossibleTractions!);
        Utilities.UpdateCollection(existingCar!.PossibleEngines!, car!.PossibleEngines!);
        
        context.SaveChanges();
        return car;
    }

    public bool DeleteCar(Car car)
    {
        context.Cars.Remove(car);
        context.SaveChanges();
        return true;
    }

    public bool DoesCarExist(int id)
    {
        return context.Cars.Any(c => c.Id == id);
    }
}