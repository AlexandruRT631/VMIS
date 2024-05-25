using listing_backend.Entities;

namespace listing_backend.Services;

public interface IModelService
{
    public List<Model> GetAllModels();
    public Model? GetModelById(int id);
    public Model CreateModel(Model model);
    public Model UpdateModel(Model model);
    public bool DeleteModel(int id);
    public List<Model> GetModelsByMakeId(int makeId);
}