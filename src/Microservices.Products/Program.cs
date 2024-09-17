using Microservices.Products.DataAccessLayer;
using Microservices.Shared.Services;
using Microservices.Shared.Settings;
using Microsoft.EntityFrameworkCore;
using Vault;
using Vault.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var vaultSettings = builder.Services.ConfigureAndGet<VaultSettings>(builder.Configuration, nameof(VaultSettings));
var rabbitMQSettings = builder.Services.ConfigureAndGet<RabbitMQSettings>(builder.Configuration, nameof(RabbitMQSettings));

builder.Services.AddSwaggerGen();
builder.Services.AddSingleton((serviceProvider) =>
{
    var config = new VaultConfiguration(vaultSettings!.Address);
    var vaultClient = new VaultClient(config);

    vaultClient.SetToken(vaultSettings!.Token);

    return vaultClient;
});

var connectionString = DependencyInjection.GetConnectionDatabase(vaultSettings!);

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