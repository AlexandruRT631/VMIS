namespace listing_backend.Services;

public interface IImageService
{
    public string SaveImage(IFormFile file);
    public void DeleteImage(string url);
}