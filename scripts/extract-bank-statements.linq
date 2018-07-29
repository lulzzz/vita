<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>Viso</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>


List<string> AccountIds = new List<string>();

void Main()
{
	// Join with query expression.
	var results = from a in BsAccounts.Take(100)
				 join b in BsAccountTransactions on a.BsAccountId equals b.BsAccountId
				 where a.BsAccountId.Length > 5
				 select new {b.Date, Description = b.Text, Amount = b.Amount, b.Notes, b.Tags, Bank = a.Institution, AccountName = a.AccountName, AccountNumer = a.AccountNumber};
				
				 
 	results.Dump();
	
}

