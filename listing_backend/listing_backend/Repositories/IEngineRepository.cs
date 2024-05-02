using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IEngineRepository
{
    public List<Engine> GetAllEngines();
    public Engine? GetEngineById(int id);
    public Engine CreateEngine(Engine engine);
    public Engine UpdateEngine(Engine engine);
    public bool DeleteEngine(Engine engine);
    public bool DoesEngineExist(int id);
}