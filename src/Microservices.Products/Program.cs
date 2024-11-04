using Microservices.Products.DataAccessLayer;
using Microservices.Shared.Services;
using Microservices.Shared.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var vaultSettings = builder.Services.ConfigureAndGet<VaultSettings>(builder.Configuration, nameof(VaultSettings));
var rabbitMQSettings = builder.Services.ConfigureAndGet<RabbitMQSettings>(builder.Configuration, nameof(RabbitMQSettings));

builder.Services.AddSwaggerGen();

var connectionString = await DependencyInjection.ReadVaultSecretAsync(vaultSettings!, "sqlserver", "connection", "secret");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, options =>
    {
        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
    });
});

builder.Services.AddProducerRabbitMQ(rabbitMQSettings!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();