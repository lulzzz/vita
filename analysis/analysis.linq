<Query Kind="Program">
  <Connection>
    <ID>2ebabf6f-d96f-4153-9675-7a230f7e370b</ID>
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
</Query>

void Main()
{

	var converted = BankStatementReadModels
	.Where(x => x.TransactionUtcDate > DateTime.Now.AddMonths(-3)).ToList()
	.Select(x => new { Category = x.Category.ToEnum<CategoryType>(), x.Amount});
	
	var cats = from p in converted
						group p by p.Category into g
						select new { Category = g.Key, Total = converted.Where(a => a.Category == g.Key).Sum(x => x.Amount) }
				  ;

	cats.Dump("Category");

	var subcategories = from p in BankStatementReadModels
	.Where(x => x.TransactionUtcDate > DateTime.Now.AddMonths(-3))
						group p by p.SubCategory into g
						select new { SubCategory = g.Key, Total = BankStatementReadModels.Where(a => a.SubCategory == g.Key).Sum(x => x.Amount) }
				  ;

	subcategories.Dump("Subcategories");


	//	subcategories.Sum(x=>x.Total).Dump("Total spending");

	BankStatementReadModels
	.Where(x => x.SubCategory == "Uncategorised")
	.Select(x => new {x.Category,x.SubCategory,x.Description, x.Amount})
	.Dump("Uncategorised");
}