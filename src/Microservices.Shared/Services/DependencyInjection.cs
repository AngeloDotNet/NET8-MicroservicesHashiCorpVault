using MassTransit;
using Microservices.Shared.Settings;
using Microservices.Shared.Vault;
using Microsoft.Extensions.DependencyInjection;

namespace Microservices.Shared.Services;

public static class DependencyInjection
{
    public static IServiceCollection AddProducerRabbitMQ(this IServiceCollection services, RabbitMQSettings settings)
    {
        services.AddMassTransit(options =>
        {
            options.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(settings.Hostname), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });

            });
        });

        return services;
    }

    public static IServiceCollection AddConsumerRabbitMQ<Consumer>(this IServiceCollection services, RabbitMQSettings settings)
    {
        services.AddMassTransit(x =>
        {
            var consumer = typeof(Consumer).Assembly;

            x.AddConsumers(consumer);
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(settings.Hostname), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
                cfg.ReceiveEndpoint(settings.QueueReceiver, e =>
                {
                    e.ConfigureConsumers(context);
                });
            });
        });

        return services;
    }

    public static async Task GenerateVaultSecretsAsync(VaultSettings vaultSettings)
    {
        // Create secret
        var setSQLConnection = await GenerateVaultSecretAsync(vaultSettings, "sqlserver", "connection", "Data Source=[HOSTNAME];Initial Catalog=Microservices;User ID=[USERNAME];Password=[PASSWORD];Encrypt=False", "secret");

        // You can create N secrets by duplicating the command as in the following example,
        // indicating unique values ​​for PATH, KEY and VALUE for each single secret.

        // Below is an example of creating a second secret for the Redis connection string
        var setRedisConnection = await GenerateVaultSecretAsync(vaultSettings, "redis", "connection", "[HOSTNAME]:6379", "secret");
    }

    public static async Task<bool> GenerateVaultSecretAsync(VaultSettings vaultSettings, string path, string key, string value, string mountPoint)
    {
        var vaultSecretsProvider = new VaultSecretProvider(vaultSettings!.Address, vaultSettings.Token);

        var isCreated = await vaultSecretsProvider.SetSecretValueAsync(path: path, key: key, value: value, mountPoint: mountPoint);

        return isCreated;
    }

    public static async Task<string> ReadVaultSecretAsync(VaultSettings vaultSettings, string path, string key, string mountPoint)
    {
        var vaultSecretsProvider = new VaultSecretProvider(vaultSettings!.Address, vaultSettings.Token);

        return await vaultSecretsProvider.GetSecretValueAsync(path: path, key: key, mountPoint: mountPoint);
    }
}