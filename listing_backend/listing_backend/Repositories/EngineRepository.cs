using listing_backend.DataAccess;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public class EngineRepository(ListingDbContext context) : IEngineRepository
{
    public List<Engine> GetAllEngines()
    {
        return context.Engines.ToList();
    }

    public Engine? GetEngineById(int id)
    {
        return context.Engines.Find(id);
    }
    
    public Engine CreateEngine(Engine engine)
    {
        context.Engines.Add(engine);
        context.SaveChanges();
        return engine;
    }
    
    public Engine UpdateEngine(Engine engine)
    {
        context.Engines.Update(engine);
        context.SaveChanges();
        return engine;
    }
    
    public bool DeleteEngine(Engine engine)
    {
        context.Engines.Remove(engine);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesEngineExist(int id)
    {
        return context.Engines.Any(e => e.Id == id);
    }
}