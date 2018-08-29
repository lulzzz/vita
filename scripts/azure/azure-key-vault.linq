<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>Microsoft.Azure.KeyVault</NuGetReference>
  <NuGetReference>Microsoft.Azure.Management.ResourceManager.Fluent</NuGetReference>
  <NuGetReference>Microsoft.Azure.Services.AppAuthentication</NuGetReference>
  <NuGetReference>Microsoft.Extensions.Configuration.AzureKeyVault</NuGetReference>
  <NuGetReference>Microsoft.IdentityModel.Clients.ActiveDirectory</NuGetReference>
  <NuGetReference>Microsoft.Rest.ClientRuntime</NuGetReference>
  <NuGetReference>Microsoft.Rest.ClientRuntime.Azure</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Microsoft.Azure.KeyVault</Namespace>
  <Namespace>Microsoft.Azure.KeyVault.Models</Namespace>
  <Namespace>Microsoft.Azure.KeyVault.WebKey</Namespace>
  <Namespace>Microsoft.Azure.Management.ResourceManager</Namespace>
  <Namespace>Microsoft.Azure.Services.AppAuthentication</Namespace>
  <Namespace>Microsoft.Extensions.Configuration</Namespace>
  <Namespace>Microsoft.Extensions.Configuration.AzureKeyVault</Namespace>
  <Namespace>Microsoft.IdentityModel.Clients.ActiveDirectory</Namespace>
  <Namespace>Microsoft.IdentityModel.Clients.ActiveDirectory.Internal</Namespace>
  <Namespace>Microsoft.IdentityModel.Clients.ActiveDirectory.Native</Namespace>
  <Namespace>Microsoft.Rest</Namespace>
  <Namespace>Microsoft.Rest.Azure</Namespace>
  <Namespace>Microsoft.Rest.Azure.Authentication</Namespace>
  <Namespace>Microsoft.Rest.Azure.OData</Namespace>
  <Namespace>Microsoft.Rest.Serialization</Namespace>
  <Namespace>Microsoft.Rest.TransientFaultHandling</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.IO</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

/*

Retrieves secrets from Azure Key Vault and set the as Environment Variables

needs to run as admin

*/

private static HttpClient client = new HttpClient();

///https://medium.com/statuscode/getting-key-vault-secrets-in-azure-functions-37620fd20a0b
async System.Threading.Tasks.Task Main()
{ 
	
	await SetEnvironmentVariable("bankstatements-service-apiurl-test","bankstatements-service-apiurl");
	await SetEnvironmentVariable("bankstatements-service-apikey-test","bankstatements-service-apikey");
	await SetEnvironmentVariable("bankstatements-service-prefix-test","bankstatements-service-prefix");

	await SetEnvironmentVariable("bankstatements-anz-test-username");
	await SetEnvironmentVariable("bankstatements-anz-test-password");

	await SetEnvironmentVariable("bankstatements-cba-test-username");
	await SetEnvironmentVariable("bankstatements-cba-test-password");

	//
	await SetEnvironmentVariable("SpreadsheetGearLicense");


}

private async Task SetEnvironmentVariable(string name, string envName = null) {

	if (envName == null) envName = name;
	
	AzureServiceTokenProvider azureServiceTokenProvider = new AzureServiceTokenProvider();
	var kvClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback), client);
	var value = await kvClient.GetSecretAsync("https://chargeid-prod.vault.azure.net/", name);
	Console.WriteLine($"{name} = {value.Value}");
	Environment.SetEnvironmentVariable(envName, value.Value, EnvironmentVariableTarget.Machine);
}

private static async Task GetSecretFromKeyVault(AzureServiceTokenProvider azureServiceTokenProvider)
{
	KeyVaultClient keyVaultClient =
		new KeyVaultClient(
			new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));

	//Console.WriteLine("Please enter the key vault name");

	var keyvaultName ="chargeid-prod";

	try
	{
		var secret = await keyVaultClient
			.GetSecretAsync($"https://{keyvaultName}.vault.azure.net/secrets/secret")
			.ConfigureAwait(false);

		Console.WriteLine($"Secret: {secret.Value}");

	}
	catch (Exception exp)
	{
		Console.WriteLine($"Something went wrong: {exp.Message}");
	}

}

private static async Task GetResourceGroups(AzureServiceTokenProvider azureServiceTokenProvider)
{
	Console.WriteLine($"{Environment.NewLine}{Environment.NewLine}Please enter the subscription Id");

	var subscriptionId = "";

	try
	{
		var serviceCreds = new TokenCredentials(await azureServiceTokenProvider.GetAccessTokenAsync("https://management.azure.com/").ConfigureAwait(false));

		var resourceManagementClient =
			new Microsoft.Azure.Management.ResourceManager.Fluent.ResourceManagementClient(serviceCreds) { SubscriptionId = subscriptionId };

		//resourceManagementClient.ResourceGroups.Dump();
	}
	catch (Exception exp)
	{
		Console.WriteLine($"Something went wrong: {exp.Message}");
	}

}




// Requires the following Azure NuGet packages and related dependencies:
// package id="Microsoft.Azure.Management.Authorization" version="2.0.0"
// package id="Microsoft.Azure.Management.ResourceManager" version="1.4.0-preview"
// package id="Microsoft.Rest.ClientRuntime.Azure.Authentication" version="2.2.8-preview"