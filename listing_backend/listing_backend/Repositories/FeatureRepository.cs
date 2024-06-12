using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class FeatureRepository(ListingDbContext context) : IFeatureRepository
{
    public List<Feature> GetAllFeatures()
    {
        return context.Features
            .ToList();
    }

    public Feature? GetFeatureById(int id)
    {
        return context.Features
            .FirstOrDefault(f => f.Id == id);
    }
    
    public Feature CreateFeature(Feature feature)
    {
        context.Features.Add(feature);
        context.SaveChanges();
        return feature;
    }
    
    public Feature UpdateFeature(Feature feature)
    {
        context.Features.Update(feature);
        context.SaveChanges();
        return feature;
    }
    
    public bool DeleteFeature(Feature feature)
    {
        context.Features.Remove(feature);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesFeatureExist(int id)
    {
        return context.Features.Any(f => f.Id == id);
    }
    
    public bool DoesFeatureExist(string name)
    {
        return context.Features.Any(f => f.Name == name);
    }
}