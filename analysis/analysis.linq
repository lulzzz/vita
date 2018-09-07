<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Vita</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <Output>DataGrids</Output>
</Query>

void Main()
{
	var subcategories = from p in BankStatementReadModels
				  group p by p.SubCategory into g
				  select new { SubCategory = g.Key, Total = BankStatementReadModels.Where(a=>a.SubCategory==g.Key).Sum(x=>x.Amount) };
				  
	subcategories.Dump();
	
	
//	subcategories.Sum(x=>x.Total).Dump("Total spending");
	
	BankStatementReadModels.Where(x=>x.SubCategory =="Uncategorised").Dump("Uncategorised");
}
