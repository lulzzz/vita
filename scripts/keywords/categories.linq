<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference>&lt;RuntimeDirectory&gt;\System.Net.Http.dll</Reference>
  <Reference Relative="..\..\source\Vita.Contracts\bin\Debug\netstandard2.0\Vita.Contracts.dll">C:\dev\vita\source\Vita.Contracts\bin\Debug\netstandard2.0\Vita.Contracts.dll</Reference>
  <Reference Relative="..\..\source\Vita.Domain\bin\Debug\netstandard2.0\Vita.Domain.dll">C:\dev\vita\source\Vita.Domain\bin\Debug\netstandard2.0\Vita.Domain.dll</Reference>
  <Reference Relative="..\..\source\Vita.Predictor\bin\Debug\netstandard2.0\Vita.Predictor.dll">C:\dev\vita\source\Vita.Predictor\bin\Debug\netstandard2.0\Vita.Predictor.dll</Reference>
  <NuGetReference>ExtensionMinder</NuGetReference>
  <NuGetReference>GoogleApi</NuGetReference>
  <NuGetReference>Humanizer.Core</NuGetReference>
  <NuGetReference>LiteDB</NuGetReference>
  <NuGetReference>Microsoft.Azure.KeyVault</NuGetReference>
  <NuGetReference>Microsoft.Azure.Services.AppAuthentication</NuGetReference>
  <NuGetReference>Serilog</NuGetReference>
  <NuGetReference>SpreadsheetGear</NuGetReference>
  <Namespace>ExtensionMinder</Namespace>
  <Namespace>Humanizer</Namespace>
  <Namespace>Humanizer.Bytes</Namespace>
  <Namespace>Humanizer.Configuration</Namespace>
  <Namespace>Humanizer.DateTimeHumanizeStrategy</Namespace>
  <Namespace>Humanizer.Inflections</Namespace>
  <Namespace>Humanizer.Localisation</Namespace>
  <Namespace>Humanizer.Localisation.CollectionFormatters</Namespace>
  <Namespace>Humanizer.Localisation.DateToOrdinalWords</Namespace>
  <Namespace>Humanizer.Localisation.Formatters</Namespace>
  <Namespace>Humanizer.Localisation.NumberToWords</Namespace>
  <Namespace>Humanizer.Localisation.Ordinalizers</Namespace>
  <Namespace>LiteDB</Namespace>
  <Namespace>LiteDB.Shell</Namespace>
  <Namespace>Serilog</Namespace>
  <Namespace>Serilog.Configuration</Namespace>
  <Namespace>Serilog.Context</Namespace>
  <Namespace>Serilog.Core</Namespace>
  <Namespace>Serilog.Core.Enrichers</Namespace>
  <Namespace>Serilog.Data</Namespace>
  <Namespace>Serilog.Debugging</Namespace>
  <Namespace>Serilog.Events</Namespace>
  <Namespace>Serilog.Filters</Namespace>
  <Namespace>Serilog.Formatting</Namespace>
  <Namespace>Serilog.Formatting.Display</Namespace>
  <Namespace>Serilog.Formatting.Json</Namespace>
  <Namespace>Serilog.Formatting.Raw</Namespace>
  <Namespace>Serilog.Parsing</Namespace>
  <Namespace>Vita.Contracts</Namespace>
</Query>

// write out categories and subcategories 
// build the solution - as this gets it from the assembly manifest

void Main()
{
	PrintFactory();
	
	
	

}

void PrintFactory()
{
	var data = new Vita.Predictor.TextClassifiers.SpreadSheets.KeywordsSpreadsheet().LoadData();
	var cats = data.GroupBy(item => item.CategoryType)
		.Select(group => new { CategoryType = group.Key, Items = group.ToList() })
		.ToList();
	cats.Dump();
	var sb = new StringBuilder();

	sb.AppendLine("public static CategoryType FromSubcategory(string subcategory){");

	sb.AppendLine("    switch(subcategory)");
	sb.AppendLine("    {");


	foreach (var cat in cats)
	{

		foreach (var item in cat.Items)
		{
			sb.AppendLine("        case '" + item.SubCategory.Clean() + "':");
		}
		
		sb.AppendLine("            return CategoryType." + cat.CategoryType.ToString() + ";");
	}
	sb.AppendLine("        default: return CategoryType." + CategoryType.Uncategorised + ";");
	sb.AppendLine("        }");
	sb.AppendLine("    }");
	Console.WriteLine(sb.ToString().Replace("'", "\""));


}

void PrintCategories()
{

	var data = new Vita.Predictor.TextClassifiers.SpreadSheets.KeywordsSpreadsheet().LoadData();
	var cats = data.GroupBy(item => item.CategoryType)
		.Select(group => new { CategoryType = group.Key.Humanize(), Items = group.ToList() })
		.ToList();
	cats.Dump();
	var sb = new StringBuilder();

	foreach (var cat in cats)
	{

		sb.AppendLine($"public static class {cat.CategoryType}");
		sb.AppendLine("{");
		foreach (var item in cat.Items)
		{
			sb.AppendLine($"public const string {item.SubCategory.Clean()} = '{item.SubCategory.Clean()}';");
		}
		sb.AppendLine("}");
	}

	Console.WriteLine(sb.ToString().Replace("'", "\""));
}




