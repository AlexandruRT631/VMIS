using System.Net.Mail;
using user_backend.Config;
using user_backend.Constants;
using user_backend.DTOs;
using user_backend.Exceptions;
using user_backend.Repositories;

namespace user_backend.Services;

public class EmailService(
    IUserRepository userRepository, 
    SmtpConfig smtpConfig,
    IConfiguration configuration,
    IHttpClientFactory httpClientFactory
) : IEmailService
{
    public bool SendEmailUpdateListing(int listingId)
    {
        if (listingId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidListing);
        }
        var httpClient = httpClientFactory.CreateClient("listing_backend");
        ListingDto? listingDto;
        try
        {
            listingDto = httpClient.GetFromJsonAsync<ListingDto>($"/api/Listing/{listingId}")
                .GetAwaiter().GetResult();
        }
        catch (Exception)
        {
            throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
        }
        if (listingDto == null)
        {
            throw new ObjectNotFoundException(ExceptionMessages.ListingNotFound);
        }

        var emails = userRepository.GetEmailsByListingInFavourites(listingId);
        var smtpClient = new SmtpClient(smtpConfig.Host)
        {
            Port = smtpConfig.Port,
            Credentials = new System.Net.NetworkCredential(smtpConfig.Username, smtpConfig.Password),
            EnableSsl = smtpConfig.EnableSsl
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpConfig.Username!),
            Subject = EmailMessages.UpdateListingSubject,
            Body = EmailMessages.UpdateListingBody + configuration["frontendUrl"] + "listing/" + listingId,
            IsBodyHtml = true
        };
        foreach (var email in emails)
        {
            mailMessage.To.Add(email);
        }
        try 
        {
            smtpClient.Send(mailMessage);
        }
        catch (Exception e)
        {
            throw new EmailException(e.Message);
        }
        return true;
    }

    public bool SendEmailNewListing(int sellerId, int listingId)
    {
        if (sellerId <= 0)
        {
            throw new InvalidArgumentException(ExceptionMessages.InvalidListing);
        }
        if (!userRepository.DoesUserExist(sellerId))
        {
            throw new ObjectNotFoundException(ExceptionMessages.UserNotFound);
        }
        var user = userRepository.GetUserById(sellerId);

        var emails = userRepository.GetEmailsByUserInFavourites(sellerId);
        var smtpClient = new SmtpClient(smtpConfig.Host)
        {
            Port = smtpConfig.Port,
            Credentials = new System.Net.NetworkCredential(smtpConfig.Username, smtpConfig.Password),
            EnableSsl = smtpConfig.EnableSsl
        };
        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpConfig.Username!),
            Subject = EmailMessages.NewListingUserSubject,
            Body = EmailMessages.NewListingUserBody + configuration["frontendUrl"] + "listing/" + listingId + 
                   " (Profile: " + user!.Name + ")",
            IsBodyHtml = true
        };
        foreach (var email in emails)
        {
            mailMessage.To.Add(email);
        }
        try 
        {
            smtpClient.Send(mailMessage);
        }
        catch (Exception e)
        {
            throw new EmailException(e.Message);
        }
        return true;
    }
}