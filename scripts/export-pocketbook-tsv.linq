<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>viso-20180420</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Reference Relative="..\source\Vita.Domain\bin\Debug\netstandard2.0\Vita.Contracts.dll">C:\dev\vita\source\Vita.Domain\bin\Debug\netstandard2.0\Vita.Contracts.dll</Reference>
  <Reference Relative="..\source\Vita.Domain\bin\Debug\netstandard2.0\Vita.Domain.dll">C:\dev\vita\source\Vita.Domain\bin\Debug\netstandard2.0\Vita.Domain.dll</Reference>
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>ExtensionMinder</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>CsvHelper.Expressions</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
  <Namespace>ExtensionMinder</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Contracts.ChargeId</Namespace>
  <Namespace>Vita.Contracts.SubCategories</Namespace>
</Query>

/*
extract date,description, amount, subcategory

*/

void Main()
{
	var data = ImportPocketBookData.Select(x => new {TxDate = x.Date, x.Description, x.Amount, x.Bank, x.Category });
	//data.Dump();

	const string delimiter = "\t";

	var builder = new StringBuilder();
	foreach (var row in data)
	{
		var splits = row.Category.Split('-');
		CategoryType cat = CategoryType.Uncategorised;
		string subcat = "Uncategorised";
		if (splits.Length > 1)
		{
			cat = splits[0].Clean().Replace(" ", string.Empty).Trim().ToEnum<CategoryType>();
			subcat = splits[1].Clean().Replace(" ", string.Empty).Trim();
		}

		builder.AppendLine($@"{row.Bank} {delimiter} {row.Description} {delimiter} {row.Amount} {delimiter} {row.Bank} {delimiter} {row.TxDate} {delimiter} {cat} {delimiter} {subcat}");
	}
	
	builder.ToString().Dump();
	const string filePath = @"c:\temp\xxx.tsv";
	File.WriteAllText(filePath, builder.ToString());
}