<Query Kind="Program">
  <Connection>
    <ID>c5e63f91-715c-4cdc-b87c-cc24ace9a884</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>viso-20180420</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	
	var results = from p in ImportBankVisionData
				  group p.CatKeyword by p.CatName into g
				  select new { CatName = g.Key, Keywords = g.ToList().Distinct() };
				  
				  results.Dump();
}

// Define other methods and classes here
