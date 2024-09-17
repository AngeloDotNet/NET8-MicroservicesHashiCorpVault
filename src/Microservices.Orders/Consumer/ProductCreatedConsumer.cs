using MassTransit;
using Microservices.Orders.DataAccessLayer;
using Microservices.Orders.DataAccessLayer.Entities;
using Microservices.Shared.RabbitMQ;

namespace Microservices.Orders.Consumer;

public class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly ApplicationDbContext dbContext;

    public ProductCreatedConsumer(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        var newProduct = new OrderProduct
        {
            Id = context.Message.Id,
            Name = context.Message.Name
        };

        dbContext.OrderProducts.Add(newProduct);
        await dbContext.SaveChangesAsync();
    }
}
