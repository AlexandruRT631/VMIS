using listing_backend.Entities;

namespace listing_backend.Services;

public interface ICarService
{
    public List<Car> GetAllCars();
    public Car? GetCarById(int id);
    public Car CreateCar(Car car);
    public Car UpdateCar(Car car);
    public bool DeleteCar(int id);
}