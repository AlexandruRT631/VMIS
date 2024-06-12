using listing_backend.Constants;
using listing_backend.Entities;
using listing_backend.Exceptions;
using listing_backend.Repositories;

namespace listing_backend.Services;

public class FeatureService(IFeatureRepository featureRepository) : IFeatureService
{
    public List<Feature> GetAllFeatures()
    {
        return featureRepository.GetAllFeatures();
    }

    public Feature? GetFeatureById(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureRepository.DoesFeatureExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureNotFound);
        }
        
        return featureRepository.GetFeatureById(id);
    }
    
    public Feature CreateFeature(Feature feature)
    {
        if (feature == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFeature);
        }
        if (feature.Id < 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (featureRepository.DoesFeatureExist(feature.Id))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureAlreadyExists);
        }
        if (string.IsNullOrWhiteSpace(feature.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (featureRepository.DoesFeatureExist(feature.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureAlreadyExists);
        }
        
        return featureRepository.CreateFeature(feature);
    }
    
    public Feature UpdateFeature(Feature feature)
    {
        if (feature == null)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFeature);
        }
        if (feature.Id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureRepository.DoesFeatureExist(feature.Id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureNotFound);
        }
        if (string.IsNullOrWhiteSpace(feature.Name))
        {
            throw new InvalidArgumentException(ExceptionMessages.RequiredName);
        }
        if (featureRepository.DoesFeatureExist(feature.Name))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FeatureAlreadyExists);
        }
        
        return featureRepository.UpdateFeature(feature);
    }
    
    public bool DeleteFeature(int id)
    {
        if (id <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidId);
        }
        if (!featureRepository.DoesFeatureExist(id))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FeatureNotFound);
        }
        
        var feature = featureRepository.GetFeatureById(id);
        return featureRepository.DeleteFeature(feature!);
    }
}