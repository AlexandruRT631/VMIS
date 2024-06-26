using user_backend.DataAccess;
using user_backend.Entities;

namespace user_backend.Repositories;

public class FavouriteRepository(UserDbContext context) : IFavouriteRepository
{
    public bool AddFavouriteListing(int userId, int favouriteListingId)
    {
        var user = context.Users.Find(userId);
        context.FavouriteListings.Add(new FavouriteListing
        {
            User = user!,
            FavouriteListingId = favouriteListingId
        });
        context.SaveChanges();
        return true;
    }

    public bool RemoveFavouriteListing(int userId, int favouriteListingId)
    {
        var favouriteListing = context.FavouriteListings.FirstOrDefault(
            fl => fl.User.Id == userId && fl.FavouriteListingId == favouriteListingId
        );
        context.FavouriteListings.Remove(favouriteListing!);
        context.SaveChanges();
        return true;
    }

    public bool DoesFavouriteListingExist(int userId, int favouriteListingId)
    {
        return context.FavouriteListings.Any(
            fl => fl.User.Id == userId && fl.FavouriteListingId == favouriteListingId
        );
    }

    public bool RemoveListingFromFavourites(int favouriteListingId)
    {
        context.FavouriteListings.RemoveRange(
            context.FavouriteListings.Where(fl => fl.FavouriteListingId == favouriteListingId)
        );
        context.SaveChanges();
        return true;
    }

    public bool RemoveListingsFromFavourites(List<int> favouriteListingIds)
    {
        context.FavouriteListings.RemoveRange(
            context.FavouriteListings.Where(fl => favouriteListingIds.Contains(fl.FavouriteListingId))
        );
        context.SaveChanges();
        return true;
    }

    public bool AddFavouriteUser(int userId, int favouriteUserId)
    {
        var user = context.Users.Find(userId);
        context.FavouriteUsers.Add(new FavouriteUser
        {
            User = user!,
            FavouriteUserId = favouriteUserId
        });
        context.SaveChanges();
        return true;
    }

    public bool RemoveFavouriteUser(int userId, int favouriteUserId)
    {
        var favouriteUser = context.FavouriteUsers.FirstOrDefault(
            fu => fu.User.Id == userId && fu.FavouriteUserId == favouriteUserId
        );
        context.FavouriteUsers.Remove(favouriteUser!);
        context.SaveChanges();
        return true;
    }

    public bool DoesFavouriteUserExist(int userId, int favouriteUserId)
    {
        return context.FavouriteUsers.Any(
            fu => fu.User.Id == userId && fu.FavouriteUserId == favouriteUserId
        );
    }

    public bool RemoveUserFromFavourites(int favouriteUserId)
    {
        context.FavouriteUsers.RemoveRange(
            context.FavouriteUsers.Where(fu => fu.FavouriteUserId == favouriteUserId)
        );
        context.SaveChanges();
        return true;
    }
}