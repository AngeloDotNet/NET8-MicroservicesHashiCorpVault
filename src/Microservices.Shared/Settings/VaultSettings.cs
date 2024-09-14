namespace Microservices.Shared.Settings;

public class VaultSettings
{
    public string Address { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string MountPath { get; set; } = null!;
    public string SecretKey { get; set; } = null!;
}
