using listing_backend.Entities;

namespace listing_backend.Services;

public interface IImageService
{
    public string SaveImage(IFormFile file, Car car);
    public void DeleteImage(string url);
    public string MoveImage(string url, Car car);
}