using listing_backend.Entities;

namespace listing_backend.Services;

public interface IColorService
{
    public List<Color> GetAllColors();
    public Color? GetColorById(int id);
    public Color CreateColor(Color color);
    public Color UpdateColor(Color color);
    public bool DeleteColor(int id);
}