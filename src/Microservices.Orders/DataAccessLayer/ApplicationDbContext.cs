using Microservices.Orders.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace Microservices.Orders.DataAccessLayer;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderProduct> OrderProducts { get; set; }
}