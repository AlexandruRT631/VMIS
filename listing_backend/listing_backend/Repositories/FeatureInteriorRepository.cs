using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class FeatureInteriorRepository(ListingDbContext context) : IFeatureInteriorRepository
{
    public List<FeatureInterior> GetAllFeaturesInterior()
    {
        return context.FeaturesInterior.ToList();
    }

    public FeatureInterior? GetFeatureInteriorById(int id)
    {
        return context.FeaturesInterior.Find(id);
    }
    
    public FeatureInterior CreateFeatureInterior(FeatureInterior featureInterior)
    {
        context.FeaturesInterior.Add(featureInterior);
        context.SaveChanges();
        return featureInterior;
    }
    
    public FeatureInterior UpdateFeatureInterior(FeatureInterior featureInterior)
    {
        context.FeaturesInterior.Update(featureInterior);
        context.SaveChanges();
        return featureInterior;
    }
    
    public bool DeleteFeatureInterior(FeatureInterior featureInterior)
    {
        context.FeaturesInterior.Remove(featureInterior);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesFeatureInteriorExist(int id)
    {
        return context.FeaturesInterior.Any(e => e.Id == id);
    }
    
    public bool DoesFeatureInteriorExist(string name)
    {
        return context.FeaturesInterior.Any(e => e.Name == name);
    }
}