<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\source\Vita.Web\bin\Debug\netcoreapp2.1\Vita.Contracts.dll">C:\dev\vita\source\Vita.Web\bin\Debug\netcoreapp2.1\Vita.Contracts.dll</Reference>
  <Reference Relative="..\source\Vita.Web\bin\Debug\netcoreapp2.1\Vita.Domain.dll">C:\dev\vita\source\Vita.Web\bin\Debug\netcoreapp2.1\Vita.Domain.dll</Reference>
  <NuGetReference>CsvHelper</NuGetReference>
  <NuGetReference>ExtensionMinder</NuGetReference>
  <NuGetReference>GoogleApi</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>CsvHelper.Expressions</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
  <Namespace>ExtensionMinder</Namespace>
  <Namespace>GoogleApi</Namespace>
  <Namespace>GoogleApi.Entities</Namespace>
  <Namespace>GoogleApi.Entities.Common</Namespace>
  <Namespace>GoogleApi.Entities.Common.Converters</Namespace>
  <Namespace>GoogleApi.Entities.Common.Enums</Namespace>
  <Namespace>GoogleApi.Entities.Common.Enums.Extensions</Namespace>
  <Namespace>GoogleApi.Entities.Common.Extensions</Namespace>
  <Namespace>GoogleApi.Entities.Interfaces</Namespace>
  <Namespace>GoogleApi.Entities.Maps</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Common</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Common.Enums</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Directions.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Directions.Response</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Directions.Response.Enums</Namespace>
  <Namespace>GoogleApi.Entities.Maps.DistanceMatrix.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.DistanceMatrix.Response</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Elevation.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Elevation.Response</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.Address.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.Common</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.Common.Enums</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.Location.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.Place.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.PlusCode.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geocoding.PlusCode.Response</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geolocation.Request</Namespace>
  <Namespace>GoogleApi.Entities.Maps.Geolocation.Request.Enums</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Contracts.ChargeId</Namespace>
  <Namespace>Vita.Contracts.SubCategories</Namespace>
</Query>

const string path = @"C:\dev\vita\data\data-sample.csv";

void Main()
{
	var cats = new List<CategoryType>();
	var subs = new List<string>();
	
	var list = new List<string>();
	using (StreamReader sr = new StreamReader(path))
	using (var csv = new CsvReader(sr))
	{
		//string sss = sr.ReadToEnd();
		csv.Configuration.Delimiter = ",";
		csv.Configuration.HeaderValidated = null;
		csv.Configuration.HasHeaderRecord = true;
		csv.Configuration.IgnoreBlankLines = true;
		// csv.Configuration.MissingFieldFound = null;
		csv.Configuration.TrimOptions = TrimOptions.Trim;
		//csv.Configuration.TypeConverterCache.<string>(new SubCategoryTypeConverter());

		//csv.Configuration.RegisterClassMap<BankStatement>();

		var records = csv.GetRecords<BankStatement>();

		foreach (var r in records) {
			cats.Add(Vita.Domain.Services.TextClassifiers.CategoryTypeConverter.Convert(r.category));
			subs.Add(Vita.Domain.Services.TextClassifiers.CategoryTypeConverter.ExtractSubCategory(r.category));
		}
		
		cats.Count.Dump("cats");
		subs.Count.Dump("subs");

		var d1 = cats.GroupBy(x => x).Select(x => new {Category = x.Key, Total = x.Count()}).Dump();
		var d2 = subs.GroupBy(x => x).Select(x => new {SubCategory = x.Key, Total = x.Count()}).Dump();

		string file1 =  @"c:/dev/vita/data/subs.csv";

		using (var textWriter = new StreamWriter(file1))
		{
			var writer = new CsvWriter(textWriter);
			writer.Configuration.Delimiter = ",";

			foreach (var item in d2)
			{
				writer.WriteField(item.SubCategory);
				writer.WriteField(item.Total);
				writer.NextRecord();
			}
		}

	}

}

void ReplaceStuff()
{
	string contents = "";
	var list = new List<string>();
	using (StreamReader sr = new StreamReader(path))
	using (var csv = new CsvReader(sr))
	{
		//string sss = sr.ReadToEnd();
		csv.Configuration.Delimiter = ",";
		csv.Configuration.HeaderValidated = null;
		csv.Configuration.HasHeaderRecord = true;
		csv.Configuration.IgnoreBlankLines = true;
		// csv.Configuration.MissingFieldFound = null;
		csv.Configuration.TrimOptions = TrimOptions.Trim;
		//csv.Configuration.TypeConverterCache.<string>(new SubCategoryTypeConverter());

		//csv.Configuration.RegisterClassMap<BankStatement>();

		var records = csv.GetRecords<BankStatement>();

		//records.ToList().Dump();	

		var dist = records.ToList().Select(x => x.accountname);
		contents = File.ReadAllText(path);

		list.AddRange(dist);
	}
	int acc = 0;

	contents = contents.Replace("Albemarle", "Investment Property");
	contents.Dump();
	File.WriteAllText(path, contents);
}

public class BankStatement
{
	public string date { get; set; }
	public string description { get; set; }
	public string category { get; set; }
	public string amount { get; set; }
	public string notes { get; set; }
	public string tags { get; set; }
	public string bank { get; set; }
	public string accountname { get; set; }
	public string accountnumber { get; set; }

}