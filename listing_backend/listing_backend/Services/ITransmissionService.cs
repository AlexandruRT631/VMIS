using listing_backend.Entities;

namespace listing_backend.Services;

public interface ITransmissionService
{
    public List<Transmission> GetAllTransmissions();
    public Transmission? GetTransmissionById(int id);
    public Transmission CreateTransmission(Transmission transmission);
    public Transmission UpdateTransmission(Transmission transmission);
    public bool DeleteTransmission(int id);
}