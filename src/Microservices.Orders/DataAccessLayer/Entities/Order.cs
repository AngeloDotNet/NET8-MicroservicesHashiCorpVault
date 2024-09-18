namespace Microservices.Orders.DataAccessLayer.Entities;

public class Order
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int ProductId { get; set; }
    public DateTime? OrderDate { get; set; }
}