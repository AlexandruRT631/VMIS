using listing_backend.DTOs;
using listing_backend.Entities;

namespace listing_backend.Repositories;

public interface IListingRepository
{
    public List<Listing> GetAllListings(int pageIndex, int pageSize);
    public Listing? GetListingById(int id);
    public Listing CreateListing(Listing listing);
    public Listing UpdateListing(Listing listing);
    public bool DeleteListing(Listing listing);
    public bool DoesListingExist(int id);
    public List<Listing> GetListingsBySearch(ListingSearchDto listingSearchDto, int pageIndex, int pageSize);
}