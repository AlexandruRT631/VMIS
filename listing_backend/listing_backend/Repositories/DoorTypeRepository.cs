using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class DoorTypeRepository(ListingDbContext context) : IDoorTypeRepository
{
    public List<DoorType> GetAllDoorTypes()
    {
        return context.DoorTypes.ToList();
    }

    public DoorType? GetDoorTypeById(int id)
    {
        return context.DoorTypes.Find(id);
    }
    
    public DoorType CreateDoorType(DoorType doorType)
    {
        context.DoorTypes.Add(doorType);
        context.SaveChanges();
        return doorType;
    }
    
    public DoorType UpdateDoorType(DoorType doorType)
    {
        context.DoorTypes.Update(doorType);
        context.SaveChanges();
        return doorType;
    }
    
    public bool DeleteDoorType(DoorType doorType)
    {
        context.DoorTypes.Remove(doorType);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesDoorTypeExist(int id)
    {
        return context.DoorTypes.Any(e => e.Id == id);
    }
}