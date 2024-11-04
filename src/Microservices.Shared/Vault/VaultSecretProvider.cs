using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;

namespace Microservices.Shared.Vault;

public class VaultSecretProvider
{
    private readonly IVaultClient vaultClient;

    public VaultSecretProvider(string vaultUri, string vaultToken)
    {
        var authMethod = new TokenAuthMethodInfo(vaultToken);
        var vaultClientSettings = new VaultClientSettings(vaultUri, authMethod);

        vaultClient = new VaultClient(vaultClientSettings);
    }

    public async Task<string> GetSecretValueAsync(string path, string key, string mountPoint)
    {
        var secret = await vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: path, mountPoint: mountPoint);

        if (secret.Data.Data.TryGetValue(key, out var value))
        {
            return value.ToString()!;
        }

        throw new KeyNotFoundException($"Key '{key}' not found in Vault at path '{path}'");
    }

    public async Task<bool> SetSecretValueAsync(string path, string key, string value, string mountPoint)
    {
        var secret = new Dictionary<string, object>
        {
            { key, value }
        };

        var writeSecret = await vaultClient.V1.Secrets.KeyValue.V2.WriteSecretAsync(path: path, secret, checkAndSet: null, mountPoint: mountPoint);

        return true;
    }
}