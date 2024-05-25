using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IModelRepository
{
    public List<Model> GetAllModels();
    public Model? GetModelById(int id);
    public Model CreateModel(Model model);
    public Model UpdateModel(Model model);
    public bool DeleteModel(Model model);
    public bool DoesModelExist(int id);
    public bool DoesModelExist(string name);
    public List<Model> GetModelsByMakeId(int makeId);
}