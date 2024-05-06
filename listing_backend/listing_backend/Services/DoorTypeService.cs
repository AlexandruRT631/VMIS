using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class DoorTypeService(IDoorTypeRepository doorTypeRepository) : IDoorTypeService
{
    public List<DoorType> GetAllDoorTypes()
    {
        return doorTypeRepository.GetAllDoorTypes();
    }

    public DoorType? GetDoorTypeById(int id)
    {
        if (id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!doorTypeRepository.DoesDoorTypeExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound);
        }
        
        return doorTypeRepository.GetDoorTypeById(id);
    }

    public DoorType CreateDoorType(DoorType doorType)
    {
        if (doorType == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType);
        }
        if (doorType.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (doorTypeRepository.DoesDoorTypeExist(doorType.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.DoorTypeAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(doorType.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (doorTypeRepository.DoesDoorTypeExist(doorType.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.DoorTypeAlreadyExists);
        }
        
        return doorTypeRepository.CreateDoorType(doorType);
    }

    public DoorType UpdateDoorType(DoorType doorType)
    {
        if (doorType == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidDoorType);
        }
        if (doorType.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!doorTypeRepository.DoesDoorTypeExist(doorType.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound);
        }
        if (string.IsNullOrWhiteSpace(doorType.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (doorTypeRepository.DoesDoorTypeExist(doorType.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.DoorTypeAlreadyExists);
        }
        
        return doorTypeRepository.UpdateDoorType(doorType);
    }

    public bool DeleteDoorType(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!doorTypeRepository.DoesDoorTypeExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.DoorTypeNotFound);
        }
        
        var doorType = doorTypeRepository.GetDoorTypeById(id);
        return doorTypeRepository.DeleteDoorType(doorType!);
    }
}