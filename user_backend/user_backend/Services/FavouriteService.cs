using user_backend.Constants;
using user_backend.Exceptions;
using user_backend.Repositories;

namespace user_backend.Services;

public class FavouriteService(
    IFavouriteRepository favouriteRepository,
    IUserRepository userRepository
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
        // TODO: Check if favourite listing exists
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
        if (!userRepository.DoesUserExist(userId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        // TODO: Check if favourite listing exists
        if (!favouriteRepository.DoesFavouriteListingExist(userId, favouriteListingId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteNotFound);
        }
        
        return favouriteRepository.RemoveFavouriteListing(userId, favouriteListingId);
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
        if (!userRepository.DoesUserExist(userId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        if (!userRepository.DoesUserExist(favouriteUserId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteUserNotFound);
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
        if (!userRepository.DoesUserExist(userId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        if (!userRepository.DoesUserExist(favouriteUserId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteUserNotFound);
        }
        if (!favouriteRepository.DoesFavouriteUserExist(userId, favouriteUserId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.FavouriteNotFound);
        }
        
        return favouriteRepository.RemoveFavouriteUser(userId, favouriteUserId);
    }
}