using Microservices.Orders.Consumer;
using Microservices.Orders.DataAccessLayer;
using Microservices.Shared.Services;
using Microservices.Shared.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var vaultSettings = builder.Services.ConfigureAndGet<VaultSettings>(builder.Configuration, nameof(VaultSettings));
var rabbitMQSettings = builder.Services.ConfigureAndGet<RabbitMQSettings>(builder.Configuration, nameof(RabbitMQSettings));

builder.Services.AddSwaggerGen();

// Command to be used only the first time in order to generate secrets in the Vault
await DependencyInjection.GenerateVaultSecretsAsync(vaultSettings!);

var connectionString = await DependencyInjection.ReadVaultSecretAsync(vaultSettings!, "sqlserver", "connection", "secret");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString, options =>
    {
        options.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
    });
});

builder.Services.AddConsumerRabbitMQ<ProductCreatedConsumer>(rabbitMQSettings!);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();