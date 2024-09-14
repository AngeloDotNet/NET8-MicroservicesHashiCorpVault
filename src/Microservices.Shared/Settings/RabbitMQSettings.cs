namespace Microservices.Shared.Settings;

public class RabbitMQSettings
{
    public string Hostname { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string QueueReceiver { get; set; } = null!;
}