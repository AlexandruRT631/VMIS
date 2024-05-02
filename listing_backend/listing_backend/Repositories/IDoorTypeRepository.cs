using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IDoorTypeRepository
{
    public List<DoorType> GetAllDoorTypes();
    public DoorType? GetDoorTypeById(int id);
    public DoorType CreateDoorType(DoorType doorType);
    public DoorType UpdateDoorType(DoorType doorType);
    public bool DeleteDoorType(DoorType doorType);
    public bool DoesDoorTypeExist(int id);
}