<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <NuGetReference>NSwag.CodeGeneration</NuGetReference>
  <NuGetReference>NSwag.CodeGeneration.CSharp</NuGetReference>
  <NuGetReference>NSwag.CodeGeneration.TypeScript</NuGetReference>
  <NuGetReference>NSwag.Core.Yaml</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>NJsonSchema</Namespace>
  <Namespace>NJsonSchema.Annotations</Namespace>
  <Namespace>NJsonSchema.CodeGeneration</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.CSharp</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.CSharp.Models</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.Models</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.TypeScript</Namespace>
  <Namespace>NJsonSchema.CodeGeneration.TypeScript.Models</Namespace>
  <Namespace>NJsonSchema.Converters</Namespace>
  <Namespace>NJsonSchema.Generation</Namespace>
  <Namespace>NJsonSchema.Generation.TypeMappers</Namespace>
  <Namespace>NJsonSchema.Infrastructure</Namespace>
  <Namespace>NJsonSchema.References</Namespace>
  <Namespace>NJsonSchema.Validation</Namespace>
  <Namespace>NSwag</Namespace>
  <Namespace>NSwag.CodeGeneration</Namespace>
  <Namespace>NSwag.CodeGeneration.CSharp</Namespace>
  <Namespace>NSwag.CodeGeneration.CSharp.Models</Namespace>
  <Namespace>NSwag.CodeGeneration.Models</Namespace>
  <Namespace>NSwag.CodeGeneration.OperationNameGenerators</Namespace>
  <Namespace>NSwag.Collections</Namespace>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Net.PeerToPeer</Namespace>
  <Namespace>System.Net.PeerToPeer.Collaboration</Namespace>
  <Namespace>System.Net.Sockets</Namespace>
  <Namespace>System.Runtime.Serialization</Namespace>
  <Namespace>System.Runtime.Serialization.Configuration</Namespace>
  <Namespace>System.Runtime.Serialization.Json</Namespace>
  <Namespace>System.Xml</Namespace>
</Query>

/*
Generate TypeScript or C# client for ChargeId Swagger API
https://github.com/RSuter/NSwag
Outputs .ts or .cs which can be copied into a project for consuming the API
*/

const string SwaggerApiJsonUrl = @"http://chargeid-test.azurewebsites.net/swagger/v1/swagger.json";

async void Main()
{
	var document = await SwaggerDocument.FromUrlAsync(SwaggerApiJsonUrl);
	var yaml = SwaggerYamlDocument.ToYaml(document);
	
	// paste the below into https://editor.swagger.io
	// then on that website goto --> generate client --> angular typescript
	yaml.Dump();
}

private void CreateTs(SwaggerDocument document)
{
	var settings = new NSwag.CodeGeneration.TypeScript.SwaggerToTypeScriptClientGeneratorSettings()
	{
		ClassName = "VitaApi",
		GenerateClientInterfaces = true,
		GenerateClientClasses = true,


	};
	var generator = new NSwag.CodeGeneration.TypeScript.SwaggerToTypeScriptClientGenerator(document, settings);
	var code = generator.GenerateFile();
	System.IO.File.WriteAllText(@"C:\temp\VitaApi.ts", code);
}

private void CreateCSharpClient(SwaggerDocument doc)
{
	var settings = new SwaggerToCSharpClientGeneratorSettings
	{
		ClassName = "Client",
		CSharpGeneratorSettings =
		{
			Namespace = "Vita.Api",
		}
	};
	settings.GenerateClientInterfaces = true;
	settings.GenerateClientClasses = true;
	settings.GenerateResponseClasses = true;
	settings.GenerateExceptionClasses = true;
	// settings.GenerateSyncMethods = true;
	var generator = new SwaggerToCSharpClientGenerator(doc, settings);
	var code = generator.GenerateFile();
	System.IO.File.WriteAllText(@"C:\temp\ApiClient.cs", code);
}