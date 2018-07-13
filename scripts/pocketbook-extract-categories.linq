<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>viso-20180420</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Output>DataGrids</Output>
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
</Query>

void Main()
{
	//BsAccountAnalysisGroupItemTransactions.Where(x=>x.Text.Contains("goodlife")).Dump();
	
	//Bankvision_keywords.Where(x=>x.Description.Contains("goodlife")).Dump();
	
	//Pocketbook_exports.Where(x=>x.Description.Contains("coles")).Dump();
	
	PrintJson();
	return;
	var data = ImportPocketBookData.Select(x=>x.Category).Distinct();
	var list = new List<KeyValuePair<string,string>>();
	
	foreach (var item in data)
	{	
		//Console.WriteLine("--------------------------------------------------");
		KeyValuePair<string,string> kvp;
		if (item.Contains("-")) {
			var split = item.Split(Convert.ToChar("-"));
			//Console.WriteLine(split[0]);
			//Console.WriteLine(split[1]);
			kvp = new KeyValuePair<string, string>(split[0], split[1]);
		}
		else
		{
		//	Console.WriteLine(item);
			kvp = new KeyValuePair<string, string>(item, "");
		}
		
		list.Add(kvp);
	}

	JsonConvert.SerializeObject(list).Dump();
	list.Dump();
}

private void PrintJson()
{
	const string json = @"[{'Key':'Income ','Value':' Deposits'},{'Key':'Health & Beauty ','Value':' Eyewear'},{'Key':'Health & Beauty ','Value':' Other Health & Beauty'},{'Key':'Holiday & Travel ','Value':' Flights'},{'Key':'Shopping ','Value':' Other Shopping'},{'Key':'Insurance ','Value':' Health & Life Insurance'},{'Key':'Health & Beauty ','Value':' Gyms & Fitness'},{'Key':'Household Utilities ','Value':' Phone & Internet'},{'Key':'Home ','Value':' Rates'},{'Key':'Transport ','Value':' Taxi & Rideshare'},{'Key':'Health & Beauty ','Value':' Doctors & Dentist'},{'Key':'Banking & Finance ','Value':' Interest'},{'Key':'Uncategorised','Value':''},{'Key':'Kids ','Value':' Childcare'},{'Key':'Work & Study ','Value':' Books & Stationery'},{'Key':'Shopping ','Value':' Jewellery'},{'Key':'Transport ','Value':' Parking & Tolls'},{'Key':'Entertainment ','Value':' Betting & Lotteries'},{'Key':'Home ','Value':' Services & Trades'},{'Key':'Food & Drinks ','Value':' Takeaways'},{'Key':'Banking & Finance ','Value':' Investment'},{'Key':'Health & Beauty ','Value':' Chemists'},{'Key':'Work & Study ','Value':' Other Work & Study'},{'Key':'Entertainment ','Value':' Other Entertainment'},{'Key':'Income ','Value':' Salary & Wages'},{'Key':'Transferring Money ','Value':' Other Transferring Money'},{'Key':'Holiday & Travel ','Value':' Other Travel'},{'Key':'Entertainment ','Value':' Events'},{'Key':'Transport ','Value':' Other Transport'},{'Key':'Insurance ','Value':' Home Insurance'},{'Key':'Banking & Finance ','Value':' Fees'},{'Key':'Banking & Finance ','Value':' Other Banking & Finance'},{'Key':'Entertainment ','Value':' Movies'},{'Key':'Household Utilities ','Value':' Electricity & Gas'},{'Key':'Shopping ','Value':' Electronics & Appliances'},{'Key':'Holiday & Travel ','Value':' Hotels & Accomodation'},{'Key':'Groceries ','Value':' Supermarkets'},{'Key':'Banking & Finance ','Value':' ATM Withdrawals'},{'Key':'Household Utilities ','Value':' Water'},{'Key':'Groceries ','Value':' Liquor Stores'},{'Key':'Miscellaneous ','Value':' Other'},{'Key':'Entertainment ','Value':' Media Subscriptions'},{'Key':'Shopping ','Value':' Furniture'},{'Key':'Groceries ','Value':' Other Groceries'},{'Key':'Shopping ','Value':' Sports & Outdoor'},{'Key':'Home ','Value':' Home Improvement'},{'Key':'Transferring Money ','Value':' Credit Card'},{'Key':'Food & Drinks ','Value':' Cafes & Coffee'},{'Key':'Transport ','Value':' Fuel'},{'Key':'Health & Beauty ','Value':' Hair & Beauty'},{'Key':'Food & Drinks ','Value':' Bars & Pubs'},{'Key':'Groceries ','Value':' Convenience Stores'},{'Key':'Work & Study ','Value':' News & Subscriptions'},{'Key':'Transport ','Value':' Public Transport'},{'Key':'Shopping ','Value':' Clothing & Fashion'},{'Key':'Food & Drinks ','Value':' Other Food & Drinks'},{'Key':'Banking & Finance ','Value':' Taxes & Fines'},{'Key':'Food & Drinks ','Value':' Restaurants'},{'Key':'Insurance ','Value':' Car Insurance'},{'Key':'Banking & Finance ','Value':' Loan Repayments'},{'Key':'Transport ','Value':' Vehicle Maintenance'},{'Key':'Kids ','Value':' School Fees'},{'Key':'Income ','Value':' Other Income'},{'Key':'Miscellaneous ','Value':' Charity'},{'Key':'Kids ','Value':' Activities & Entertainment'}]";
	var cats = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(json);
	var data = cats.Select(x => x.Value).Distinct();
	foreach (var d in data.OrderBy(x=>x))
	{
		Console.WriteLine($"[Description('{d.Trim()}')]{d.Replace("&",string.Empty).Replace(" ", string.Empty)},");
	}
}