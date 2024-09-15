namespace Microservices.Shared.Models;

public class DeserializerDto
{
    public object data { get; set; } = null!;
    public MetadataDto metadata { get; set; } = null!;
}

public class MetadataDto
{
    public string created_time { get; set; } = null!;
    public object custom_metadata { get; set; } = null!;
    public string deletion_time { get; set; } = null!;
    public bool destroyed { get; set; }
    public int version { get; set; }
}
