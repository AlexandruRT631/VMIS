using listing_backend.Entities;

namespace listing_backend.Services;

public interface IFeatureExteriorService
{
    public List<FeatureExterior> GetAllFeaturesExterior();
    public FeatureExterior? GetFeatureExteriorById(int id);
    public FeatureExterior CreateFeatureExterior(FeatureExterior featureExterior);
    public FeatureExterior UpdateFeatureExterior(FeatureExterior featureExterior);
    public bool DeleteFeatureExterior(int id);
}