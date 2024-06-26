using user_backend.Constants;
using user_backend.DTOs;
using user_backend.Entities;
using user_backend.Exceptions;
using user_backend.Repositories;

namespace user_backend.Services;

public class FavouriteService(
    IFavouriteRepository favouriteRepository,
    IUserRepository userRepository,
    IHttpClientFactory httpClientFactory
) : IFavouriteService
{
    public bool AddFavouriteListing(int userId, int favouriteListingId)
    {
        if (userId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (favouriteListingId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavorite);
        }
        if (!userRepository.DoesUserExist(userId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        var httpClient = httpClientFactory.CreateClient("listing_backend");
        ListingDto? listingDto;
        try
        {
            listingDto = httpClient.GetFromJsonAsync<ListingDto>($"/api/Listing/{favouriteListingId}")
                .GetAwaiter().GetResult();
        }
        catch (Exception)
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteListingNotFound);
        }
        if (listingDto == null)
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteListingNotFound);
        }
        if (listingDto.SellerId == userId)
        {
            throw new InvalidArgumentException(ExceptionMessages.FavouriteListingSellerConflict);
        }
        if (favouriteRepository.DoesFavouriteListingExist(userId, favouriteListingId))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FavouriteAlreadyExists);
        }
        
        return favouriteRepository.AddFavouriteListing(userId, favouriteListingId);
    }

    public bool RemoveFavouriteListing(int userId, int favouriteListingId)
    {
        if (userId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (favouriteListingId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavorite);
        }
        if (!favouriteRepository.DoesFavouriteListingExist(userId, favouriteListingId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteNotFound);
        }
        
        return favouriteRepository.RemoveFavouriteListing(userId, favouriteListingId);
    }

    public bool RemoveListingFromFavourites(int favouriteListingId)
    {
        if (favouriteListingId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavorite);
        }
        var httpClient = httpClientFactory.CreateClient("listing_backend");
        ListingDto? listingDto;
        try
        {
            listingDto = httpClient.GetFromJsonAsync<ListingDto>($"/api/Listing/{favouriteListingId}")
                .GetAwaiter().GetResult();
        }
        catch (Exception)
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteListingNotFound);
        }
        if (listingDto == null)
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteListingNotFound);
        }
        
        return favouriteRepository.RemoveListingFromFavourites(favouriteListingId);
    }

    public bool RemoveListingsFromFavourites(List<int> favouriteListingIds)
    {
        if (favouriteListingIds.Count == 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavorite);
        }
        
        return favouriteRepository.RemoveListingsFromFavourites(favouriteListingIds);
    }

    public bool AddFavouriteUser(int userId, int favouriteUserId)
    {
        if (userId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (favouriteUserId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavorite);
        }

        if (userId == favouriteUserId)
        {
            throw new InvalidArgumentException(ExceptionMessages.FavouriteUserConflict);
        }
        if (!userRepository.DoesUserExist(userId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        if (!userRepository.DoesUserExist(favouriteUserId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteUserNotFound);
        }
        var favouriteUser = userRepository.GetUserById(favouriteUserId);
        if (favouriteUser!.Role != UserRole.Seller && favouriteUser!.Role != UserRole.Admin)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavouriteUser);
        }
        if (favouriteRepository.DoesFavouriteUserExist(userId, favouriteUserId))
        {
            throw new ObjectAlreadyExistsException(ExceptionMessages.FavouriteAlreadyExists);
        }
        
        return favouriteRepository.AddFavouriteUser(userId, favouriteUserId);
    }

    public bool RemoveFavouriteUser(int userId, int favouriteUserId)
    {
        if (userId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidUser);
        }
        if (favouriteUserId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidFavorite);
        }
        if (!favouriteRepository.DoesFavouriteUserExist(userId, favouriteUserId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteNotFound);
        }
        
        return favouriteRepository.RemoveFavouriteUser(userId, favouriteUserId);
    }
}