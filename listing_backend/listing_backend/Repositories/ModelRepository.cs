using listing_backend.DataAccess;
using listing_backend.Entities;
using Microsoft.EntityFrameworkCore;

namespace listing_backend.Repositories;

public class ModelRepository(ListingDbContext context) : IModelRepository
{
    public List<Model> GetAllModels()
    {
        return context.Models
            .ToList();
    }

    public Model? GetModelById(int id)
    {
        return context.Models
            .FirstOrDefault(m => m.Id == id);
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
    
    public bool DoesModelExist(string name)
    {
        return context.Models.Any(e => e.Name == name);
    }
    
    public List<Model> GetModelsByMakeId(int makeId)
    {
        return context.Models
            .Where(m => m.Make!.Id == makeId)
            .ToList();
    }
}