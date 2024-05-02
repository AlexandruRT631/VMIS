using listing_backend.Entities;

namespace listing_backend.Services;

public interface ITractionService
{
    public List<Traction> GetAllTractions();
    public Traction? GetTractionById(int id);
    public Traction CreateTraction(Traction traction);
    public Traction UpdateTraction(Traction traction);
    public bool DeleteTraction(int id);
}