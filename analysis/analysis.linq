<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Vita</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Output>DataGrids</Output>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Api.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Api.dll</Reference>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll</Reference>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll</Reference>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll</Reference>
  <NuGetReference>ExtensionMinder</NuGetReference>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>ExtensionMinder</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Contracts.ChargeId</Namespace>
  <Namespace>Vita.Contracts.SubCategories</Namespace>
  <Namespace>Vita.Domain.Infrastructure</Namespace>
</Query>

// default one year
public DateTime FromUtcDateTime { get; set; } = DateTime.UtcNow.AddYears(-1);
public DateTime ToUtcDateTime { get; set; } = DateTime.UtcNow;

void Main()
{

	var readModels = BankStatementReadModels.Where(x => x.TransactionUtcDate > FromUtcDateTime && x.TransactionUtcDate < ToUtcDateTime).ToList();

	var cats = from p in readModels
			   group p by p.Category
	  into g
			   select new
			   {
				   Category = g.Key,
				   Total = readModels.Where(a => a.Category == g.Key)
				 .Sum(x => x.Amount)
			   };
			   
	cats.Dump("cats");

	var subs = from p in readModels.Where(x => !string.IsNullOrEmpty(x.SubCategory))
			   group p by p.SubCategory
	  into g
			   select new
			   {
				   SubCategory = g.Key,
				   Total = readModels.Where(a => a.SubCategory == g.Key)
				 .Sum(x => x.Amount)
			   };


	subs.Dump("subs");

	var unmatched = from p in readModels.Where(x=>x.SubCategory == Categories.Uncategorised).Dump("unmatched")
			   group p by p.SubCategory
	  into g
			   select new
			   {
				   SubCategory = g.Key,
				   Total = readModels.Where(a => a.SubCategory == g.Key)
				 .Sum(x => x.Amount)
			   };
	
	unmatched.Dump("unmatched-totals");

}