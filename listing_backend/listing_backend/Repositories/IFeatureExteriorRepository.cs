using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IFeatureExteriorRepository
{
    public List<FeatureExterior> GetAllFeaturesExterior();
    public FeatureExterior? GetFeatureExteriorById(int id);
    public FeatureExterior CreateFeatureExterior(FeatureExterior featureExterior);
    public FeatureExterior UpdateFeatureExterior(FeatureExterior featureExterior);
    public bool DeleteFeatureExterior(FeatureExterior featureExterior);
    public bool DoesFeatureExteriorExist(int id);
    public bool DoesFeatureExteriorExist(string name);
}