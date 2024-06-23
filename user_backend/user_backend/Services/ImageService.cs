namespace user_backend.Services;

public class ImageService(IConfiguration configuration) : IImageService
{
    private readonly string _filesFolderPath = configuration["FilesFolderPath"]!;
    private readonly string _profilePicturesFolderName = "profilePictures";
    
    public string SaveImage(IFormFile image)
    {
        var profilePicturePath = Path.Combine(_filesFolderPath, _profilePicturesFolderName);
        var fileName = Path.GetRandomFileName() + Path.GetExtension(image.FileName);
        var filePath = Path.Combine(profilePicturePath, fileName);
        if (!Directory.Exists(_filesFolderPath))
        {
            Directory.CreateDirectory(_filesFolderPath);
        }
        
        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            image.CopyTo(fileStream);
        }

        return $"/{_profilePicturesFolderName}/{fileName}";
    }

    public void DeleteImage(string url)
    {
        var filePath = _filesFolderPath + url;
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public string GetDefaultImageUrl()
    {
        return $"/{_profilePicturesFolderName}/default.jpg";
    }
}