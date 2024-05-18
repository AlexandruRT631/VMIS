using listing_backend.Entities;

namespace listing_backend.Services;

public interface IListingService
{
    public List<Listing> GetAllListings();
    public Listing? GetListingById(int id);
    public Listing CreateListing(Listing listing, List<IFormFile>? images);
    public Listing UpdateListing(Listing listing, List<IFormFile>? images);
    public bool DeleteListing(int id);
}