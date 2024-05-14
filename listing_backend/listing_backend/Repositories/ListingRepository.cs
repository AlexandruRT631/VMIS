using listing_backend.DataAccess;
using listing_backend.Entities;
using listing_backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace listing_backend.Repositories;

public class ListingRepository(ListingDbContext context) : IListingRepository
{
    public List<Listing> GetAllListings()
    {
        return context.Listings
            .ToList();
    }

    public Listing? GetListingById(int id)
    {
        return context.Listings
            .Include(existingListing => existingListing.FeaturesExterior!)
            .Include(existingListing => existingListing.FeaturesInterior!)
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
            .FirstOrDefault(e => e.Id == listing.Id);
        
        context.Entry(existingListing!).CurrentValues.SetValues(listing);
        
        Utilities.UpdateCollection(existingListing!.FeaturesExterior!, listing!.FeaturesExterior!);
        Utilities.UpdateCollection(existingListing!.FeaturesInterior!, listing!.FeaturesInterior!);
        
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