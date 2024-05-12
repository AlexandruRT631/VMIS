using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class TractionService(ITractionRepository tractionRepository) : ITractionService
{
    public List<Traction> GetAllTractions()
    {
        return tractionRepository.GetAllTractions();
    }

    public Traction? GetTractionById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!tractionRepository.DoesTractionExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound);
        }
        
        return tractionRepository.GetTractionById(id);
    }

    public Traction CreateTraction(Traction traction)
    {
        if (traction == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTraction);
        }
        if (traction.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (tractionRepository.DoesTractionExist(traction.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.TractionAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(traction.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (tractionRepository.DoesTractionExist(traction.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.TractionAlreadyExists);
        }
        
        return tractionRepository.CreateTraction(traction);
    }

    public Traction UpdateTraction(Traction traction)
    {
        
        if (traction == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTraction);
        }
        if (traction.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!tractionRepository.DoesTractionExist(traction.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound);
        }
        if (string.IsNullOrWhiteSpace(traction.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (tractionRepository.DoesTractionExist(traction.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.TractionAlreadyExists);
        }
        
        return tractionRepository.UpdateTraction(traction);
    }

    public bool DeleteTraction(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!tractionRepository.DoesTractionExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TractionNotFound);
        }
        
        var traction = tractionRepository.GetTractionById(id);
        return tractionRepository.DeleteTraction(traction!);
    }
}