namespace user_backend.Services;

public interface IEmailService
{
    public bool SendEmailUpdateListing(int listingId);
    public bool SendEmailNewListing(int sellerId, int listingId);
    public void SendEmailNewConversation(int userId);
    public void SendEmailNewMessage(int userId);
}