namespace user_backend.Repositories;

public interface IFavouriteRepository
{
    public bool AddFavouriteListing(int userId, int favouriteListingId);
    public bool RemoveFavouriteListing(int userId, int favouriteListingId);
    public bool DoesFavouriteListingExist(int userId, int favouriteListingId);
    public bool RemoveListingFromFavourites(int favouriteListingId);
    public bool RemoveListingsFromFavourites(List<int> favouriteListingIds);
    public bool AddFavouriteUser(int userId, int favouriteUserId);
    public bool RemoveFavouriteUser(int userId, int favouriteUserId);
    public bool DoesFavouriteUserExist(int userId, int favouriteUserId);
    public bool RemoveUserFromFavourites(int favouriteUserId);
}