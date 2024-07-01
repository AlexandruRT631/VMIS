namespace user_backend.DTOs;

public class UserDetailsDto
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Role { get; set; }
    public List<int>? FavouriteListings { get; set; }
    public List<int>? FavouriteUsers { get; set; }
    public List<ConversationDto>? Conversations { get; set; }
}