using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IMakeRepository
{
    public List<Make> GetAllMakes();
    public Make? GetMakeById(int id);
    public Make CreateMake(Make make);
    public Make UpdateMake(Make make);
    public bool DeleteMake(Make make);
    public bool DoesMakeExist(int id);
}