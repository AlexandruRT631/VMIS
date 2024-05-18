namespace listing_backend.Services;

public class ImageService(IConfiguration configuration) : IImageService
{
    private readonly string _imageFolderPath = configuration["ImageFolderPath"]!;
    
    public string SaveImage(IFormFile image)
    {
        var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(_imageFolderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        return $"/images/{fileName}";
    }

    public void DeleteImage(string url)
    {
        var filePath = Path.Combine(_imageFolderPath, Path.GetFileName(url));
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}