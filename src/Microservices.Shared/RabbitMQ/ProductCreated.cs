namespace Microservices.Shared.RabbitMQ;

public class ProductCreated
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}