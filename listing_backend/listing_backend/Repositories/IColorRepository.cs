using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IColorRepository
{
    public List<Color> GetAllColors();
    public Color? GetColorById(int id);
    public Color CreateColor(Color color);
    public Color UpdateColor(Color color);
    public bool DeleteColor(Color color);
    public bool DoesColorExist(int id);
    public bool DoesColorExist(string name);
}