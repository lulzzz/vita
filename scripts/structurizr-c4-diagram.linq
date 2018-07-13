<Query Kind="Program">
  <NuGetReference>OpenSoftware.Structurizr.Dgml</NuGetReference>
  <NuGetReference>Structurizer</NuGetReference>
  <NuGetReference>Structurizr.Core</NuGetReference>
  <Namespace>EnsureThat</Namespace>
  <Namespace>JetBrains.Annotations</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>OpenSoftware.DgmlTools</Namespace>
  <Namespace>OpenSoftware.DgmlTools.Analyses</Namespace>
  <Namespace>OpenSoftware.DgmlTools.Builders</Namespace>
  <Namespace>OpenSoftware.DgmlTools.Model</Namespace>
  <Namespace>OpenSoftware.Structurizr.Dgml</Namespace>
  <Namespace>Structurizer</Namespace>
  <Namespace>Structurizer.Configuration</Namespace>
  <Namespace>Structurizer.Schemas</Namespace>
  <Namespace>Structurizr</Namespace>
  <Namespace>Structurizr.Analysis</Namespace>
  <Namespace>Structurizr.Api</Namespace>
  <Namespace>Structurizr.Core.Util</Namespace>
  <Namespace>Structurizr.Documentation</Namespace>
  <Namespace>Structurizr.Encryption</Namespace>
  <Namespace>Structurizr.IO</Namespace>
  <Namespace>Structurizr.IO.Json</Namespace>
  <Namespace>Structurizr.IO.PlantUML</Namespace>
  <Namespace>Structurizr.Util</Namespace>
  <Namespace>System</Namespace>
  <Namespace>System.ComponentModel.Composition</Namespace>
  <Namespace>System.ComponentModel.Composition.Hosting</Namespace>
  <Namespace>System.ComponentModel.Composition.Primitives</Namespace>
  <Namespace>System.ComponentModel.Composition.ReflectionModel</Namespace>
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>System.Diagnostics.Tracing</Namespace>
  <Namespace>System.IO.Compression</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Numerics</Namespace>
  <Namespace>System.Reflection</Namespace>
</Query>

void Main()
{
	Workspace workspace = new Workspace("RedTrout", "Architecture overviews");
	Model model = workspace.Model;

	Person user = model.AddPerson("User", "API user.");
	SoftwareSystem softwareSystem = model.AddSoftwareSystem("ChargeId System", "architecture");
	user.Uses(softwareSystem, "Uses");

	ViewSet viewSet = workspace.Views;
	SystemContextView contextView = viewSet.CreateSystemContextView(softwareSystem, "SystemContext", "System Context diagram.");
	contextView.AddAllSoftwareSystems();
	contextView.AddAllPeople();

	Styles styles = viewSet.Configuration.Styles;
	styles.Add(new ElementStyle(Tags.SoftwareSystem) { Background = "#1168bd", Color = "#ffffff" });
	styles.Add(new ElementStyle(Tags.Person) { Background = "#08427b", Color = "#ffffff", Shape = Shape.Person });


	//	StructurizrClient structurizrClient = new StructurizrClient("76b16c85-c278-48fa-a3b9-6ae614ed7b97 ", "732839af-5786-42f8-836b-13ea350ac059");
	//	structurizrClient.PutWorkspace(38996, workspace);

	// Convert to DGML
	var dgml = workspace.ToDgml();
	// Write to file
	
	dgml.WriteToFile(System.IO.Path.Combine(@"c:\temp", "c4model.dgml"));
	

}

// Define other methods and classes here