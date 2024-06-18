using System.Text;
using listing_backend.DataAccess;
using listing_backend.DTOs;
using listing_backend.Entities;
using listing_backend.Utils;
using Microsoft.EntityFrameworkCore;

namespace listing_backend.Repositories;

public class ListingRepository(ListingDbContext context) : IListingRepository
{
    public (List<Listing>, int) GetAllListings(int pageIndex, int pageSize)
    {
        var orderedListings = context.Listings
            .OrderBy(l => l.CreatedAt);
        var totalPages = (int) Math.Ceiling(orderedListings.Count() / (double) pageSize);
        var listings = QueryUtilities.Paginate(orderedListings, pageIndex, pageSize);
        return (listings.ToList(), totalPages);
    }

    public Listing? GetListingById(int id)
    {
        return context.Listings
            .Include(existingListing => existingListing.Features!)
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
            .Include(existingListing => existingListing.Features!)
            .Include(existingListing => existingListing.ListingImages!)
            .FirstOrDefault(e => e.Id == listing.Id);
        
        context.Entry(existingListing!).CurrentValues.SetValues(listing);
        
        Utilities.UpdateCollection(existingListing!.Features!, listing!.Features!);
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

    public (List<Listing>, int) GetListingsBySearch(ListingSearchDto listingSearchDto, int pageIndex, int pageSize)
    {
        var listings = from l in context.Listings
            select l;

        if (!string.IsNullOrWhiteSpace(listingSearchDto.Keywords))
        {
            var keywords = listingSearchDto.Keywords.Split(' ');
            listings = keywords.Aggregate(
                listings,
                (current, keyword) => QueryUtilities.AddContains(
                    current,
                    l => l.Title,
                    keyword
                ));
        }
        
        if (listingSearchDto.ModelId != null)
        {
            listings = QueryUtilities.AddEqual(
                listings,
                l => l.Car!.Model!.Id,
                listingSearchDto.ModelId.Value
            );
        }
        else if (listingSearchDto.MakeId != null)
        {
            listings = QueryUtilities.AddEqual(
                listings,
                l => l.Car!.Model!.Make!.Id,
                listingSearchDto.MakeId.Value
            );
        }
        
        if (listingSearchDto.StartYear != null)
        {
            listings = QueryUtilities.AddEqual(
                listings,
                l => l.Car!.StartYear,
                listingSearchDto.StartYear.Value
            );
        }
        
        if (listingSearchDto.MinYear != null)
        {
            listings = QueryUtilities.AddGreaterOrEqual(
                listings,
                l => l.Year,
                listingSearchDto.MinYear.Value
            );
        }
        
        if (listingSearchDto.MaxYear != null)
        {
            listings = QueryUtilities.AddLessOrEqual(
                listings,
                l => l.Year,
                listingSearchDto.MaxYear.Value
            );
        }
        
        if (listingSearchDto.MinMileage != null)
        {
            listings = QueryUtilities.AddGreaterOrEqual(
                listings,
                l => l.Mileage,
                listingSearchDto.MinMileage.Value
            );
        }
        
        if (listingSearchDto.MaxMileage != null)
        {
            listings = QueryUtilities.AddLessOrEqual(
                listings,
                l => l.Mileage,
                listingSearchDto.MaxMileage.Value
            );
        }
        
        if (listingSearchDto.Categories is { Count: > 0 })
        {
            listings = QueryUtilities.AddIn(
                listings,
                l => l.Category!.Id,
                listingSearchDto.Categories
            );
        }
        
        if (listingSearchDto.Fuels is { Count: > 0 })
        {
            listings = QueryUtilities.AddIn(
                listings,
                l => l.Engine!.Fuel!.Id,
                listingSearchDto.Fuels
            );
        }
        
        if (listingSearchDto.MinPower != null)
        {
            listings = QueryUtilities.AddGreaterOrEqual(
                listings,
                l => l.Engine!.Power,
                listingSearchDto.MinPower.Value
            );
        }
        
        if (listingSearchDto.MaxPower != null)
        {
            listings = QueryUtilities.AddLessOrEqual(
                listings,
                l => l.Engine!.Power,
                listingSearchDto.MaxPower.Value
            );
        }
        
        if (listingSearchDto.MinTorque != null)
        {
            listings = QueryUtilities.AddGreaterOrEqual(
                listings,
                l => l.Engine!.Torque,
                listingSearchDto.MinTorque.Value
            );
        }
        
        if (listingSearchDto.MaxTorque != null)
        {
            listings = QueryUtilities.AddLessOrEqual(
                listings,
                l => l.Engine!.Torque,
                listingSearchDto.MaxTorque.Value
            );
        }
        
        if (listingSearchDto.MinDisplacement != null)
        {
            listings = QueryUtilities.AddGreaterOrEqual(
                listings,
                l => l.Engine!.Displacement,
                listingSearchDto.MinDisplacement.Value
            );
        }
        
        if (listingSearchDto.MaxDisplacement != null)
        {
            listings = QueryUtilities.AddLessOrEqual(
                listings,
                l => l.Engine!.Displacement,
                listingSearchDto.MaxDisplacement.Value
            );
        }
        
        if (!string.IsNullOrWhiteSpace(listingSearchDto.EngineCode))
        {
            listings = QueryUtilities.AddEqual(
                listings,
                l => l.Engine!.EngineCode,
                listingSearchDto.EngineCode
            );
        }
        
        if (listingSearchDto.DoorTypes is { Count: > 0 })
        {
            listings = QueryUtilities.AddIn(
                listings,
                l => l.DoorType!.Id,
                listingSearchDto.DoorTypes
            );
        }

        if (listingSearchDto.Transmissions is { Count: > 0 })
        {
            listings = QueryUtilities.AddIn(
                listings,
                l => l.Transmission!.Id,
                listingSearchDto.Transmissions
            );
        }
        
        if (listingSearchDto.Tractions is { Count: > 0 })
        {
            listings = QueryUtilities.AddIn(
                listings,
                l => l.Traction!.Id,
                listingSearchDto.Tractions
            );
        }
        
        if (listingSearchDto.Colors is { Count: > 0 })
        {
            listings = QueryUtilities.AddIn(
                listings,
                l => l.Color!.Id,
                listingSearchDto.Colors
            );
        }
        
        if (listingSearchDto.Features is { Count: > 0 })
        {
            listings = listingSearchDto.Features.Aggregate(
                listings,
                (current, feature) => QueryUtilities.AddContains(
                    current,
                    l => l.Features!.Select(f => f.Id).ToList(),
                    feature
                ));
        }
        
        if (listingSearchDto.MinPrice != null)
        {
            listings = QueryUtilities.AddGreaterOrEqual(
                listings,
                l => l.Price,
                listingSearchDto.MinPrice.Value
            );
        }

        if (listingSearchDto.MaxPrice != null)
        {
            listings = QueryUtilities.AddLessOrEqual(
                listings,
                l => l.Price,
                listingSearchDto.MaxPrice.Value
            );
        }
        
        var totalPages = (int) Math.Ceiling(listings.Count() / (double) pageSize);
        listings = QueryUtilities.Paginate(listings.OrderBy(l => l.CreatedAt), pageIndex, pageSize);
        return (listings.ToList(), totalPages);
    }
}