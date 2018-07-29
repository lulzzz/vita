<Query Kind="Program">
  <Output>DataGrids</Output>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Contracts.dll</Reference>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Domain.dll</Reference>
  <Reference Relative="..\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll">C:\dev\vita\source\Vita.Api\bin\Debug\netcoreapp2.1\Vita.Predictor.dll</Reference>
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
  <Namespace>System.Diagnostics</Namespace>
  <Namespace>Vita.Contracts</Namespace>
  <Namespace>Vita.Contracts.ChargeId</Namespace>
  <Namespace>Vita.Contracts.SubCategories</Namespace>
  <Namespace>Vita.Domain.Infrastructure</Namespace>
  <Namespace>Vita.Domain.Infrastructure.Importers</Namespace>
  <Namespace>Vita.Predictor</Namespace>
  <Namespace>Vita.Predictor.TextClassifiers</Namespace>
</Query>

const string path = @"C:\dev\vita\data\data-sample.csv";
public static string Data = PredictorSettings.GetFilePath("data-sample.csv", false);
public static string Train = PredictorSettings.GetFilePath("train.csv", false, false);
public static string Test = PredictorSettings.GetFilePath("test.csv", false, false)
;

void Main()
{
	var data = PocketBookImporter.Import(Data);

	int percent = (int)(data.Count() * .8);

	data = data.Shuffle();

	var train = data.Take(percent);
	var test = data.Except(train);

	var train1 = FileUtil.Create(train);
	File.WriteAllText(Train, train1);

	var test1 = FileUtil.Create(test);
	File.WriteAllText(Test, test1);

	var traintsv = FileUtil.Read(train1);
	Debug.Assert(traintsv.Count() == train.Count());

	var testtsv = FileUtil.Read(test1);
	Debug.Assert(testtsv.Count() == test1.Count());
}


void MakeSubCategoriesCsv()
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
		var records = csv.GetRecords<BankStatement>();

		foreach (var r in records)
		{
			cats.Add(CategoryTypeConverter.Convert(r.category));
			subs.Add(CategoryTypeConverter.ExtractSubCategory(r.category));
		}

		cats.Count.Dump("cats");
		subs.Count.Dump("subs");

		var d1 = cats.GroupBy(x => x).Select(x => new { Category = x.Key, Total = x.Count() }).Dump();
		var d2 = subs.GroupBy(x => x).Select(x => new { SubCategory = x.Key, Total = x.Count() }).Dump();

		string file1 = @"c:/dev/vita/data/subs.csv";

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