using listing_backend.Entities;

namespace listing_backend.Services;

public class ImageService(IConfiguration configuration) : IImageService
{
    private readonly string _filesFolderPath = configuration["FilesFolderPath"]!;
    private readonly string _imagesFolderName = "images";
    
    public string SaveImage(IFormFile image, Car car)
    {
        var imageFolderPath = Path.Combine(_filesFolderPath, _imagesFolderName);
        var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
        var makePath = Path.Combine(imageFolderPath, car.Model!.Make!.Name);
        if (!Directory.Exists(makePath))
        {
            Directory.CreateDirectory(makePath);
        }
        var modelPath = Path.Combine(makePath, car.Model!.Name!);
        if (!Directory.Exists(modelPath))
        {
            Directory.CreateDirectory(modelPath);
        }
        var folderPath = Path.Combine(modelPath, car.StartYear.ToString());
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(stream);
        }

        return $"/{_imagesFolderName}/{car.Model!.Make!.Name}/{car.Model!.Name!}/{car.StartYear.ToString()}/{fileName}";
    }

    public void DeleteImage(string url)
    {
        var filePath = _filesFolderPath + url;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
    
    public string MoveImage(string url, Car car)
    {
        var oldFilePath = Path.Combine(_filesFolderPath, url.TrimStart('/'));
        var fileBytes = File.ReadAllBytes(oldFilePath);
        var fileName = Path.GetFileName(oldFilePath);
        var newFilePath = SaveImage(new FormFile(new MemoryStream(fileBytes), 0, fileBytes.Length, "image", fileName), car);
        File.Delete(oldFilePath);
        return newFilePath;
    }
}