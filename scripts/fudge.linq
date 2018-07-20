<Query Kind="Program">
  <NuGetReference>CsvHelper</NuGetReference>
  <Namespace>CsvHelper</Namespace>
  <Namespace>CsvHelper.Configuration</Namespace>
  <Namespace>CsvHelper.Configuration.Attributes</Namespace>
  <Namespace>CsvHelper.Expressions</Namespace>
  <Namespace>CsvHelper.TypeConversion</Namespace>
</Query>

void Main()
{
	const string path = @"C:\dev\vita\data\data-sample.csv";
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
		
		var dist = records.ToList().Select(x=>x.accountname);
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