using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface ITransmissionRepository
{
    public List<Transmission> GetAllTransmissions();
    public Transmission? GetTransmissionById(int id);
    public Transmission CreateTransmission(Transmission transmission);
    public Transmission UpdateTransmission(Transmission transmission);
    public bool DeleteTransmission(Transmission transmission);
    public bool DoesTransmissionExist(int id);
}