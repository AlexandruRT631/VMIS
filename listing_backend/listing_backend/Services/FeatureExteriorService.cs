using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class FeatureExteriorService(IFeatureExteriorRepository featureExteriorRepository) : IFeatureExteriorService
{
    public List<FeatureExterior> GetAllFeaturesExterior()
    {
        return featureExteriorRepository.GetAllFeaturesExterior();
    }

    public FeatureExterior? GetFeatureExteriorById(int id)
    {
        if (id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureExteriorRepository.DoesFeatureExteriorExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureExteriorNotFound);
        }
        
        return featureExteriorRepository.GetFeatureExteriorById(id);
    }
    
    public FeatureExterior CreateFeatureExterior(FeatureExterior featureExterior)
    {
        if (featureExterior == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureExterior);
        }
        if (featureExterior.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (featureExteriorRepository.DoesFeatureExteriorExist(featureExterior.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureExteriorAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(featureExterior.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (featureExteriorRepository.DoesFeatureExteriorExist(featureExterior.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureExteriorAlreadyExists);
        }
        
        return featureExteriorRepository.CreateFeatureExterior(featureExterior);
    }
    
    public FeatureExterior UpdateFeatureExterior(FeatureExterior featureExterior)
    {
        if (featureExterior == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFeatureExterior);
        }
        if (featureExterior.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureExteriorRepository.DoesFeatureExteriorExist(featureExterior.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureExteriorNotFound);
        }
        if (string.IsNullOrWhiteSpace(featureExterior.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (featureExteriorRepository.DoesFeatureExteriorExist(featureExterior.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureExteriorAlreadyExists);
        }
        
        return featureExteriorRepository.UpdateFeatureExterior(featureExterior);
    }
    
    public bool DeleteFeatureExterior(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureExteriorRepository.DoesFeatureExteriorExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureExteriorNotFound);
        }
        
        var featureExterior = featureExteriorRepository.GetFeatureExteriorById(id);
        return featureExteriorRepository.DeleteFeatureExterior(featureExterior!);
    }
}