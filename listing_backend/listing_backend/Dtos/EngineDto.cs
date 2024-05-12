namespace listing_backend.DTOs;

public class EngineDto
{
    public int Id { get; set; }
    public MakeDto? Make { get; set; }
    public string? EngineCode { get; set; }
    public int Displacement { get; set; }
    public FuelDto? Fuel { get; set; }
    public int Power { get; set; }
    public int Torque { get; set; }
}