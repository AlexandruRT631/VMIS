using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class MakeService(IMakeRepository makeRepository) : IMakeService
{
    public List<Make> GetAllMakes()
    {
        return makeRepository.GetAllMakes();
    }

    public Make? GetMakeById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!makeRepository.DoesMakeExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        
        return makeRepository.GetMakeById(id);
    }

    public Make CreateMake(Make make)
    {
        if (make == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
        }
        if (make.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (makeRepository.DoesMakeExist(make.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.MakeAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(make.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (makeRepository.DoesMakeExist(make.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.MakeAlreadyExists);
        }
        
        return makeRepository.CreateMake(make);
    }

    public Make UpdateMake(Make make)
    {
        if (make == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidMake);
        }
        if (make.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!makeRepository.DoesMakeExist(make.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        if (string.IsNullOrWhiteSpace(make.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (makeRepository.DoesMakeExist(make.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.MakeAlreadyExists);
        }
        
        return makeRepository.UpdateMake(make);
    }

    public bool DeleteMake(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!makeRepository.DoesMakeExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.MakeNotFound);
        }
        
        var make = makeRepository.GetMakeById(id);
        return makeRepository.DeleteMake(make!);
    }
}