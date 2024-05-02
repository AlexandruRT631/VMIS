using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface ICarRepository
{
    public List<Car> GetAllCars();
    public Car? GetCarById(int id);
    public Car CreateCar(Car car);
    public Car UpdateCar(Car car);
    public bool DeleteCar(Car car);
    public bool DoesCarExist(int id);
}