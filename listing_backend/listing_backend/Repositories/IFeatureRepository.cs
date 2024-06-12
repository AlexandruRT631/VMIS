using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IFeatureRepository
{
    public List<Feature> GetAllFeatures();
    public Feature? GetFeatureById(int id);
    public Feature CreateFeature(Feature feature);
    public Feature UpdateFeature(Feature feature);
    public bool DeleteFeature(Feature feature);
    public bool DoesFeatureExist(int id);
    public bool DoesFeatureExist(string name);
}