namespace user_backend.Constants;

public static class ExceptionMessages
{
    public const string UserNotFound = "User not found";
    public const string UserAlreadyExists = "User already exists";
    public const string InvalidId = "Invalid id";
    public const string InvalidUser = "Invalid user";
    public const string RequiredEmail = "Email is required";
    public const string RequiredPassword = "Password is required";
    public const string RequiredName = "Name is required";
    public const string UsedEmail = "Email is already in use";
    public const string InvalidUserRole = "Invalid user role";
    public const string InvalidCredentials = "Invalid credentials";
    public const string InvalidToken = "Invalid token";
    public const string ForbiddenUser = "Forbidden user";
    public const string RequiredRefreshToken = "Refresh token is required";
    public const string InvalidRefreshToken = "Invalid refresh token";
    public const string ExpiredRefreshToken = "Expired refresh token";
    public const string ExcludedRefreshToken = "Refresh Token field is excluded";
    public const string InvalidFavorite = "Invalid favorite";
    public const string FavouriteAlreadyExists = "Favourite already exists";
    public const string FavouriteNotFound = "Favourite not found";
    public const string FavouriteListingNotFound = "Favourite listing not found";
    public const string FavouriteUserNotFound = "Favourite user not found";
    public const string InvalidPageIndex = "Invalid page index.";
    public const string InvalidPageSize = "Invalid page size.";
    public const string InvalidFavouriteUser = "Favourite user is not seller.";
    public const string FavouriteListingSellerConflict = "Cannot add own listing to favourites.";
    public const string FavouriteUserConflict = "Cannot add yourself to favourites.";
    public const string InvalidListing = "Invalid listing.";
    public const string ListingNotFound = "Listing not found.";
    public const string InvalidConversation = "Invalid conversation.";
    public const string ConversationNotFound = "Conversation not found.";
    public const string InvalidMessage = "Invalid message.";
    public const string InvalidSender = "Invalid sender.";
    public const string SenderNotFound = "Sender not found.";
    public const string RequiredContent = "Content is required.";
    public const string ConversationAlreadyExists = "Conversation already exists.";
}