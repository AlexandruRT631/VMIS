using listing_backend.Entities;

namespace listing_backend.Services;

public interface IEngineService
{
    public List<Engine> GetAllEngines();
    public Engine? GetEngineById(int id);
    public Engine CreateEngine(Engine engine);
    public Engine UpdateEngine(Engine engine);
    public bool DeleteEngine(int id);
}