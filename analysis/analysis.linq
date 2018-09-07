<Query Kind="Program">
  <Connection>
    <ID>2ebabf6f-d96f-4153-9675-7a230f7e370b</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Vita</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var subcategories = from p in BankStatementReadModels.Where(x=>x.TransactionUtcDate > DateTime.Now.AddMonths(-3))
				  group p by p.SubCategory into g
				  select new { SubCategory = g.Key, Total = BankStatementReadModels.Where(a=>a.SubCategory==g.Key).Sum(x=>x.Amount) }
				  ;
				  
	subcategories.Dump();
	
	
//	subcategories.Sum(x=>x.Total).Dump("Total spending");
	
	BankStatementReadModels.Where(x=>x.SubCategory =="Uncategorised").Dump("Uncategorised");
}