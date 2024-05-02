using listing_backend.Entities;

namespace listing_backend.Services;

public interface IFeatureInteriorService
{
    public List<FeatureInterior> GetAllFeaturesInterior();
    public FeatureInterior? GetFeatureInteriorById(int id);
    public FeatureInterior CreateFeatureInterior(FeatureInterior featureInterior);
    public FeatureInterior UpdateFeatureInterior(FeatureInterior featureInterior);
    public bool DeleteFeatureInterior(int id);
}