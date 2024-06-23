namespace user_backend.Services;

public interface IImageService
{
    public string SaveImage(IFormFile image);
    public void DeleteImage(string url);
    public string GetDefaultImageUrl();
}