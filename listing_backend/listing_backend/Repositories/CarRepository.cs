using listing_backend.DataAccess;
using listing_backend.Entities;

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
        context.Cars.Update(car);
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