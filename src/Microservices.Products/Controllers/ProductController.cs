using MassTransit;
using Microservices.Products.DataAccessLayer;
using Microservices.Products.DataAccessLayer.Entities;
using Microservices.Shared.RabbitMQ;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Products.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;
    private readonly IPublishEndpoint publishEndpoint;

    public ProductController(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        this.dbContext = dbContext;
        this.publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        var products = await dbContext.Products.ToListAsync();
        return Ok(products);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var product = await dbContext.Products.FindAsync(id);
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Product newProduct)
    {
        dbContext.Products.Add(newProduct);
        await dbContext.SaveChangesAsync();

        await publishEndpoint.Publish<ProductCreated>(new ProductCreated
        {
            Id = newProduct.Id,
            Name = newProduct.Name
        });

        return CreatedAtAction("GET", new { id = newProduct.Id }, newProduct);
    }
}