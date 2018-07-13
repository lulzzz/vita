<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Security.dll</Reference>
  <Reference Relative="..\source\Vita.DataImporter\bin\Debug\netcoreapp2.0\Vita.Contracts.dll">C:\dev\vita\source\Vita.DataImporter\bin\Debug\netcoreapp2.0\Vita.Contracts.dll</Reference>
  <Reference Relative="..\source\Vita.DataImporter\bin\Debug\netcoreapp2.0\Vita.Domain.dll">C:\dev\vita\source\Vita.DataImporter\bin\Debug\netcoreapp2.0\Vita.Domain.dll</Reference>
  <NuGetReference>MongoDB.Driver</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>MongoDB.Bson</Namespace>
  <Namespace>MongoDB.Bson.IO</Namespace>
  <Namespace>MongoDB.Bson.Serialization</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Attributes</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Conventions</Namespace>
  <Namespace>MongoDB.Bson.Serialization.IdGenerators</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Options</Namespace>
  <Namespace>MongoDB.Bson.Serialization.Serializers</Namespace>
  <Namespace>MongoDB.Driver</Namespace>
  <Namespace>MongoDB.Driver.Core.Authentication</Namespace>
  <Namespace>MongoDB.Driver.Core.Authentication.Sspi</Namespace>
  <Namespace>MongoDB.Driver.Core.Bindings</Namespace>
  <Namespace>MongoDB.Driver.Core.Clusters</Namespace>
  <Namespace>MongoDB.Driver.Core.Clusters.ServerSelectors</Namespace>
  <Namespace>MongoDB.Driver.Core.Configuration</Namespace>
  <Namespace>MongoDB.Driver.Core.ConnectionPools</Namespace>
  <Namespace>MongoDB.Driver.Core.Connections</Namespace>
  <Namespace>MongoDB.Driver.Core.Events</Namespace>
  <Namespace>MongoDB.Driver.Core.Events.Diagnostics</Namespace>
  <Namespace>MongoDB.Driver.Core.Misc</Namespace>
  <Namespace>MongoDB.Driver.Core.Operations</Namespace>
  <Namespace>MongoDB.Driver.Core.Operations.ElementNameValidators</Namespace>
  <Namespace>MongoDB.Driver.Core.Servers</Namespace>
  <Namespace>MongoDB.Driver.Core.WireProtocol</Namespace>
  <Namespace>MongoDB.Driver.Core.WireProtocol.Messages</Namespace>
  <Namespace>MongoDB.Driver.Core.WireProtocol.Messages.Encoders</Namespace>
  <Namespace>MongoDB.Driver.Core.WireProtocol.Messages.Encoders.BinaryEncoders</Namespace>
  <Namespace>MongoDB.Driver.Core.WireProtocol.Messages.Encoders.JsonEncoders</Namespace>
  <Namespace>MongoDB.Driver.GeoJsonObjectModel</Namespace>
  <Namespace>MongoDB.Driver.GeoJsonObjectModel.Serializers</Namespace>
  <Namespace>MongoDB.Driver.Linq</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Security.Cryptography</Namespace>
  <Namespace>System.Security.Cryptography.Pkcs</Namespace>
  <Namespace>System.Security.Cryptography.X509Certificates</Namespace>
  <Namespace>System.Security.Cryptography.Xml</Namespace>
  <Namespace>System.Security.Permissions</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Contracts.ChargeId</Namespace>
</Query>

void Main()
{
	var facade = new AzureCosmosDbFacade();
	var collection = facade.GetDatabase().GetCollection<Company>("Companies");
	var totalCount= collection.Count(new BsonDocument());
	totalCount.Dump();
}

public class AzureCosmosDbFacade
{

	private MongoClient _client;


	public MongoClient CreateClient()
	{
		var connectionString = "";
		
				var settings = MongoClientSettings.FromUrl(
		  new MongoUrl(connectionString)
		);
		settings.SslSettings =
		  new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
		var client = new MongoClient(settings);
		return client;
	}

	public IMongoDatabase GetDatabase()
	{
		if (_client == null)
		{
			_client = CreateClient();
		}

		var database = CreateClient().GetDatabase("chargeid-test");
		return database;
	}

	public IMongoCollection<Vita.Domain.Search.SearchDocument> GetSearchDataCollection()
	{

		return GetDatabase().GetCollection<Vita.Domain.Search.SearchDocument>("SearchData");
	}
}