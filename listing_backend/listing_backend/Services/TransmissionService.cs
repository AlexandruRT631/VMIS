using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class TransmissionService(ITransmissionRepository transmissionRepository) : ITransmissionService
{
    public List<Transmission> GetAllTransmissions()
    {
        return transmissionRepository.GetAllTransmissions();
    }

    public Transmission? GetTransmissionById(int id)
    {
        if (id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }

        if (!transmissionRepository.DoesTransmissionExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound);
        }

        return transmissionRepository.GetTransmissionById(id);
    }

    public Transmission CreateTransmission(Transmission transmission)
    {
        if (transmission == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission);
        }

        if (transmission.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }

        if (transmissionRepository.DoesTransmissionExist(transmission.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.TransmissionAlreadyExists);
        }

        if (string.IsNullOrWhiteSpace(transmission.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }

        if (transmissionRepository.DoesTransmissionExist(transmission.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.TransmissionAlreadyExists);
        }

        return transmissionRepository.CreateTransmission(transmission);
    }

    public Transmission UpdateTransmission(Transmission transmission)
    {
        if (transmission == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidTransmission);
        }

        if (transmission.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }

        if (!transmissionRepository.DoesTransmissionExist(transmission.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound);
        }

        if (string.IsNullOrWhiteSpace(transmission.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }

        if (transmissionRepository.DoesTransmissionExist(transmission.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.TransmissionAlreadyExists);
        }

        return transmissionRepository.UpdateTransmission(transmission);
    }

    public bool DeleteTransmission(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!transmissionRepository.DoesTransmissionExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.TransmissionNotFound);
        }

        var transmission = transmissionRepository.GetTransmissionById(id);
        return transmissionRepository.DeleteTransmission(transmission!);
    }
}