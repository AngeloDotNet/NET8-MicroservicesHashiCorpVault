namespace Microservices.Orders.BusinessLayer.DTOs;

public class OrderDto
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public DateTime? OrderDate { get; set; }
    public OrderProductDto ProductInfo { get; set; } = new OrderProductDto();
}