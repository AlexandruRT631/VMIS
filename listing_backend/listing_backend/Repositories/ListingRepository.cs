using listing_backend.DataAccess;
using listing_backend.Entities;
using listing_backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace listing_backend.Repositories;

public class ListingRepository(ListingDbContext context) : IListingRepository
{
    public List<Listing> GetAllListings(int pageIndex, int pageSize)
    {
        return context.Listings
            .OrderBy(l => l.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    public Listing? GetListingById(int id)
    {
        return context.Listings
            .Include(existingListing => existingListing.FeaturesExterior!)
            .Include(existingListing => existingListing.FeaturesInterior!)
            .Include(existingListing => existingListing.ListingImages!)
            .FirstOrDefault(e => e.Id == id);
    }
    
    public Listing CreateListing(Listing listing)
    {
        context.Listings.Add(listing);
        context.SaveChanges();
        return listing;
    }
    
    public Listing UpdateListing(Listing listing)
    {
        var existingListing = context.Listings
            .Include(existingListing => existingListing.FeaturesExterior!)
            .Include(existingListing => existingListing.FeaturesInterior!)
            .Include(existingListing => existingListing.ListingImages!)
            .FirstOrDefault(e => e.Id == listing.Id);
        
        context.Entry(existingListing!).CurrentValues.SetValues(listing);
        
        Utilities.UpdateCollection(existingListing!.FeaturesExterior!, listing!.FeaturesExterior!);
        Utilities.UpdateCollection(existingListing!.FeaturesInterior!, listing!.FeaturesInterior!);
        Utilities.UpdateCollection(existingListing!.ListingImages!, listing!.ListingImages!);
        
        context.SaveChanges();
        return listing;
    }
    
    public bool DeleteListing(Listing listing)
    {
        context.Listings.Remove(listing);
        context.SaveChanges();
        return true;
    }
    
    public bool DoesListingExist(int id)
    {
        return context.Listings.Any(e => e.Id == id);
    }
}