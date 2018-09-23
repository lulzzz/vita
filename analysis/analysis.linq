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
  <Namespace>Vita.Domain.Infrastructure</Namespace>
</Query>

// default one year
public DateTime FromUtcDateTime { get; set; } = DateTime.UtcNow.AddYears(-1);
public DateTime ToUtcDateTime { get; set; } = DateTime.UtcNow;

IList<BankStatementReadModel> readModels;

void Main()
{
	readModels = BankStatementReadModels.Where(x => x.TransactionUtcDate > FromUtcDateTime && x.TransactionUtcDate < ToUtcDateTime).ToList();
	Console.WriteLine($"read models {readModels.Count()}");
	
	ShowSummary();
	ShowIncome();
	ShowLoans();

}

private void ShowSummary()
{
	var cats = from p in readModels
			   group p by p.Category into g
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

}

private void ShowIncome()
{
	var income = readModels
	.Where(x => x.Category == CategoryType.Income.GetDescription())
	.OrderBy(x=>x.TransactionUtcDate)
	.ToArray();
	
	var daysBetweenPay = new List<int>();
	
	for (int x = 0; x < income.Count() - 1; x++) {
		
		int y = x+1;
		if (y >= income.Count()) continue;
		
		var arg = (income[y].TransactionUtcDate-income[x].TransactionUtcDate).Value.Days;
		
		daysBetweenPay.Add(arg);
	}

	var totalincome = income.Sum(x=>x.Amount);
	
	double paycycle = daysBetweenPay.Average();
	var nextpay = income.Max(x=>x.TransactionUtcDate).Value.AddDays(paycycle);
	
	daysBetweenPay.Dump("days between pays");
	
	Console.WriteLine($"income - avg days between pay {daysBetweenPay.Average()}");
	Console.WriteLine($"income - nextpay {nextpay}");
	Console.WriteLine($"income - totalincome {totalincome}");
	Console.WriteLine($"income - average income {(totalincome/income.Count()).Value.TruncateDecimal(2)}");
}

private void ShowLoans() {
	
	var loans = readModels.Where(x=>x.SubCategory == "LoanRepayments");
	loans.Dump("loans");
}

private void ShowDuplicates()
{
	var duplicates = readModels.Select(x => new { x.AggregateId, x.SubCategory, x.Description, x.TransactionUtcDate, x.Amount });
	var dups = from p in duplicates
			   group p by p.Description into g
			   select new
			   {
				   Description = g.Key,
				   Total = readModels.Select(x => new { x.AggregateId, x.SubCategory, x.Description, x.TransactionUtcDate, x.Amount }).Count(a => a.Description == g.Key)
			   };

	dups.Where(x => x.Total > 1).Dump("duplicates");
	BankStatementReadModels.Where(x => x.Description == "AEG OGDEN PERTH ARENA PERTH").Dump("example duplicate");
}

private void ShowUnmatched()
{
	var unmatched = readModels.Where(x => x.SubCategory == SubCategories.Uncategorised).Dump("unmatched");
	var unmatchedtotal = from p in unmatched
						 group p by p.SubCategory
	  into g
						 select new
						 {
							 SubCategory = g.Key,
							 Total = readModels.Where(a => a.SubCategory == g.Key)
						   .Sum(x => x.Amount)
						 };

	Console.WriteLine($"unmatched {unmatched}".Dump("unmatched").Count());

	unmatched
	.Select(x => new { AggregateId = x.AggregateId, RequestId = x.RequestId, Description = x.Description.ToLower(), Amount = x.Amount, x.TransactionUtcDate })
	.OrderBy(x => x.Description);

	unmatchedtotal.Dump("unmatched-totals");
}