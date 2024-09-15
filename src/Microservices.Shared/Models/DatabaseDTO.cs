namespace Microservices.Shared.Models;

public class DatabaseDTO
{
    public string DbHostname { get; set; } = null!;
    public string DbDatabase { get; set; } = null!;
    public string DbUsername { get; set; } = null!;
    public string DbPassword { get; set; } = null!;
}