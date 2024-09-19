using Microservices.Orders.BusinessLayer.DTOs;
using Microservices.Orders.DataAccessLayer;
using Microservices.Orders.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Orders.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(ApplicationDbContext dbContext) : ControllerBase
{
    private readonly ApplicationDbContext dbContext = dbContext;

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var orders = await dbContext.Orders.Join(dbContext.OrderProducts,
            order => order.ProductId,
            product => product.Id, (order, product) => new { Order = order, Product = product }
            )
            .Select(_ => new OrderDto
            {
                Id = _.Order.Id,
                OrderDate = _.Order.OrderDate,
                UserId = _.Order.UserId,
                ProductInfo = new OrderProductDto
                {
                    Id = _.Product.Id,
                    Name = _.Product.Name
                }
            }).ToListAsync();

        return Ok(orders);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var order = await dbContext.Orders.FindAsync(id);
        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Order newOrder)
    {
        dbContext.Orders.Add(newOrder);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction("Get", new { id = newOrder.Id }, newOrder);
    }
}