namespace Microservices.Products.DataAccessLayer.Entities;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Description { get; set; } = string.Empty;
}