using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IFeatureInteriorRepository
{
    public List<FeatureInterior> GetAllFeaturesInterior();
    public FeatureInterior? GetFeatureInteriorById(int id);
    public FeatureInterior CreateFeatureInterior(FeatureInterior featureInterior);
    public FeatureInterior UpdateFeatureInterior(FeatureInterior featureInterior);
    public bool DeleteFeatureInterior(FeatureInterior featureInterior);
    public bool DoesFeatureInteriorExist(int id);
}