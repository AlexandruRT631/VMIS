using listing_backend.Entities;

namespace listing_backend.Services;

public interface IFeatureService
{
    public List<Feature> GetAllFeatures();
    public Feature? GetFeatureById(int id);
    public Feature CreateFeature(Feature feature);
    public Feature UpdateFeature(Feature feature);
    public bool DeleteFeature(int id);
}