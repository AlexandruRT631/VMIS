namespace listing_backend.DTOs;

public class ModelDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public MakeDto? Make { get; set; }
}