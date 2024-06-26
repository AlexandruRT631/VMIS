using listing_backend.DTOs;
using listing_backend.Entities;

namespace listing_backend.Services;

public interface IListingService
{
    public (List<Listing>, int) GetAllListings(int pageIndex, int pageSize);
    public Listing? GetListingById(int id);
    public Listing CreateListing(Listing listing, List<IFormFile>? images);
    public Listing UpdateListing(Listing listing, List<IFormFile>? images);
    public bool DeleteListing(int id);
    public (List<Listing>, int) GetListingsBySearch(ListingSearchDto listingSearchDto, int pageIndex, int pageSize);
    public (List<Listing>, int) GetActiveListingsByUserId(int sellerId, int pageIndex, int pageSize);
    public (List<Listing>, int) GetInactiveListingsByUserId(int sellerId, int pageIndex, int pageSize);
    public (List<Listing>, int) GetListingsByIds(List<int> ids, int pageIndex, int pageSize);
    public bool DeleteListings(int sellerId);
}