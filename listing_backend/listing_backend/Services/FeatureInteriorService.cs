using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class FeatureInteriorService(IFeatureInteriorRepository featureInteriorRepository) : IFeatureInteriorService
{
    public List<FeatureInterior> GetAllFeaturesInterior()
    {
        return featureInteriorRepository.GetAllFeaturesInterior();
    }

    public FeatureInterior? GetFeatureInteriorById(int id)
    {
        if (id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureInteriorRepository.DoesFeatureInteriorExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureInteriorNotFound);
        }
        
        return featureInteriorRepository.GetFeatureInteriorById(id);
    }
    
    public FeatureInterior CreateFeatureInterior(FeatureInterior featureInterior)
    {
        if (featureInterior == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureInterior);
        }
        if (featureInterior.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (featureInteriorRepository.DoesFeatureInteriorExist(featureInterior.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureInteriorAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(featureInterior.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (featureInteriorRepository.DoesFeatureInteriorExist(featureInterior.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureInteriorAlreadyExists);
        }
        
        return featureInteriorRepository.CreateFeatureInterior(featureInterior);
    }
    
    public FeatureInterior UpdateFeatureInterior(FeatureInterior featureInterior)
    {
        if (featureInterior == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureInterior);
        }
        if (featureInterior.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureInteriorRepository.DoesFeatureInteriorExist(featureInterior.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureInteriorNotFound);
        }
        if (string.IsNullOrWhiteSpace(featureInterior.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (featureInteriorRepository.DoesFeatureInteriorExist(featureInterior.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureInteriorAlreadyExists);
        }
        
        return featureInteriorRepository.UpdateFeatureInterior(featureInterior);
    }
    
    public bool DeleteFeatureInterior(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureInteriorRepository.DoesFeatureInteriorExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureInteriorNotFound);
        }
        
        var featureInterior = featureInteriorRepository.GetFeatureInteriorById(id);
        return featureInteriorRepository.DeleteFeatureInterior(featureInterior!);
    }
}