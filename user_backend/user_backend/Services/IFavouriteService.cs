namespace user_backend.Services;

public interface IFavouriteService
{
    public bool AddFavouriteListing(int userId, int favouriteListingId);
    public bool RemoveFavouriteListing(int userId, int favouriteListingId);
    public bool RemoveListingFromFavourites(int favouriteListingId);
    public bool RemoveListingsFromFavourites(List<int> favouriteListingIds);
    public bool AddFavouriteUser(int userId, int favouriteUserId);
    public bool RemoveFavouriteUser(int userId, int favouriteUserId);
}