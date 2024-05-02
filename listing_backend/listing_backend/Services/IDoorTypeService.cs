using listing_backend.Entities;

namespace listing_backend.Services;

public interface IDoorTypeService
{
    public List<DoorType> GetAllDoorTypes();
    public DoorType? GetDoorTypeById(int id);
    public DoorType CreateDoorType(DoorType doorType);
    public DoorType UpdateDoorType(DoorType doorType);
    public bool DeleteDoorType(int id);
}