using listing_backend.Entities;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class ColorService(IColorRepository colorRepository) : IColorService
{
    public List<Color> GetAllColors()
    {
        return colorRepository.GetAllColors();
    }

    public Color? GetColorById(int id)
    {
        return colorRepository.GetColorById(id);
    }

    public Color CreateColor(Color color)
    {
        return colorRepository.CreateColor(color);
    }

    public Color UpdateColor(Color color)
    {
        return colorRepository.UpdateColor(color);
    }

    public bool DeleteColor(int id)
    {
        var color = colorRepository.GetColorById(id);
        return colorRepository.DeleteColor(color!);
    }
}