using listing_backend.Entities;

namespace listing_backend.Services;

public interface IListingService
{
    public List<Listing> GetAllListings();
    public Listing? GetListingById(int id);
    public Listing CreateListing(Listing listing);
    public Listing UpdateListing(Listing listing);
    public bool DeleteListing(int id);
}