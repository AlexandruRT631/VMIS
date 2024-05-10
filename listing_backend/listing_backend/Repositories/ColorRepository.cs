using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class ColorRepository(ListingDbContext context) : IColorRepository
{
    public List<Color> GetAllColors()
    {
        return context.Colors
            .ToList();
    }

    public Color? GetColorById(int id)
    {
        return context.Colors
            .FirstOrDefault(c => c.Id == id);
    }

    public Color CreateColor(Color color)
    {
        context.Colors.Add(color);
        context.SaveChanges();
        return color;
    }

    public Color UpdateColor(Color color)
    {
        context.Colors.Update(color);
        context.SaveChanges();
        return color;
    }

    public bool DeleteColor(Color color)
    {
        context.Colors.Remove(color);
        context.SaveChanges();
        return true;
    }

    public bool DoesColorExist(int id)
    {
        return context.Colors.Any(e => e.Id == id);
    }
    
    public bool DoesColorExist(string name)
    {
        return context.Colors.Any(e => e.Name == name);
    }
}