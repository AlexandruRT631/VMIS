using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class ModelRepository(ListingDbContext context) : IModelRepository
{
    public List<Model> GetAllModels()
    {
        return context.Models.ToList();
    }

    public Model? GetModelById(int id)
    {
        return context.Models.Find(id);
    }
    
    public Model CreateModel(Model model)
    {
        context.Models.Add(model);
        context.SaveChanges();
        return model;
    }
    
    public Model UpdateModel(Model model)
    {
        context.Models.Update(model);
        context.SaveChanges();
        return model;
    }
    
    public bool DeleteModel(Model model)
    {
        context.Models.Remove(model);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesModelExist(int id)
    {
        return context.Models.Any(e => e.Id == id);
    }
}