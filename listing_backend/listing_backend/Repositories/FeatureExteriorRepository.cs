using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class FeatureExteriorRepository(ListingDbContext context) : IFeatureExteriorRepository
{
    public List<FeatureExterior> GetAllFeaturesExterior()
    {
        return context.FeaturesExterior.ToList();
    }

    public FeatureExterior? GetFeatureExteriorById(int id)
    {
        return context.FeaturesExterior.Find(id);
    }
    
    public FeatureExterior CreateFeatureExterior(FeatureExterior featureExterior)
    {
        context.FeaturesExterior.Add(featureExterior);
        context.SaveChanges();
        return featureExterior;
    }
    
    public FeatureExterior UpdateFeatureExterior(FeatureExterior featureExterior)
    {
        context.FeaturesExterior.Update(featureExterior);
        context.SaveChanges();
        return featureExterior;
    }
    
    public bool DeleteFeatureExterior(FeatureExterior featureExterior)
    {
        context.FeaturesExterior.Remove(featureExterior);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesFeatureExteriorExist(int id)
    {
        return context.FeaturesExterior.Any(e => e.Id == id);
    }
    
    public bool DoesFeatureExteriorExist(string name)
    {
        return context.FeaturesExterior.Any(e => e.Name == name);
    }
}