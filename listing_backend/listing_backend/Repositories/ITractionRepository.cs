using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface ITractionRepository
{
    public List<Traction> GetAllTractions();
    public Traction? GetTractionById(int id);
    public Traction CreateTraction(Traction traction);
    public Traction UpdateTraction(Traction traction);
    public bool DeleteTraction(Traction traction);
    public bool DoesTractionExist(int id);
}