using listing_backend.Entities;

namespace listing_backend.Services;

public interface IMakeService
{
    public List<Make> GetAllMakes();
    public Make? GetMakeById(int id);
    public Make CreateMake(Make make);
    public Make UpdateMake(Make make);
    public bool DeleteMake(int id);
}