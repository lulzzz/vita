using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;

namespace Vita.Domain.Infrastructure
{
    public static class SecretMan
    {
        public static async Task<string> SpreadsheetGearLicense()
        {
            async Task<string> Func()
            {
                var client = new HttpClient();
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var kvClient =
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback),
                        client);
                var bundle = await kvClient.GetSecretAsync("https://chargeid-prod.vault.azure.net/",
                    "SpreadsheetGearLicense");
                return bundle.Value;
            }

            var result = await Cacher.GetAsync("SpreadsheetGearLicense", Func, DateTimeOffset.MaxValue);
            return result;
        }


        public static string Get(string key)
        {
            return AsyncUtil.RunSync(() => GetAsync(key));
        }

        public static async Task<string> GetAsync(string key)
        {
            async Task<string> Func()
            {
                var client = new HttpClient();
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var kvClient =
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback),
                        client);
                var bundle = await kvClient.GetSecretAsync("https://chargeid-prod.vault.azure.net/", key);
                return bundle.Value;
            }

            var result = await Cacher.GetAsync(key, Func, DateTimeOffset.MaxValue);
            return result;
        }
    }
}