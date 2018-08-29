<Query Kind="Program">
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.dll</Reference>
  <Reference>&lt;RuntimeDirectory&gt;\System.Threading.Tasks.dll</Reference>
  <NuGetReference>Flurl</NuGetReference>
  <NuGetReference>Flurl.Http</NuGetReference>
  <Namespace>Flurl</Namespace>
  <Namespace>Flurl.Http</Namespace>
  <Namespace>Flurl.Http.Configuration</Namespace>
  <Namespace>Flurl.Http.Content</Namespace>
  <Namespace>Flurl.Http.Testing</Namespace>
  <Namespace>Flurl.Util</Namespace>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Bson</Namespace>
  <Namespace>Newtonsoft.Json.Converters</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
  <Namespace>Newtonsoft.Json.Schema</Namespace>
  <Namespace>Newtonsoft.Json.Serialization</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http</Namespace>
  <Namespace>System.Net.Http.Headers</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

async Task Main()
{
	try
	{
		
		string searchPhrase = @"coles";
		
		const string json = "application/json";
		var result = await "https://chargeid-search-test.search.windows.net"
		  .WithHeader("content-type", json)
		  .WithHeader("accept", json)
		  .WithHeader("api-key", "AA16F445BF48690488F8A7D2E9F24CF6")
		  .AppendPathSegment("indexes")
		  .AppendPathSegment("charges-index")
		  .AppendPathSegment("docs")
		  .AppendPathSegment("search")
		  .SetQueryParam("api-version", "2016-09-01")
		  //  .SetQueryParam("search", $"searchPhrase%3Acontains('{searchPhrase}')")
		  .SendJsonAsync(HttpMethod.Post, new
		  {
			  search = $"searchPhrase:{searchPhrase}",
			  queryType = "full",
			  searchMode = "all",
		  });

		result.EnsureSuccessStatusCode();
		var content = await result.Content.ReadAsStringAsync();
		var output = JsonConvert.DeserializeObject<SearchResult>(content);
		output.Dump();
	}
	catch (Exception e)
	{ 
		e.Dump();
	}
}

public class SearchResult
{

	[JsonProperty("@odata.context")]
	public string OdataContext { get; set; }

	[JsonProperty("value")]
	public IList<SearchResultItem> Value { get; set; }
}

public class SearchResultItem
{

	[JsonProperty("@search.score")]
	public double SearchScore { get; set; }

	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("searchPhrase")]
	public string SearchPhrase { get; set; }

	[JsonProperty("keywords")]
	public string Keywords { get; set; }

	[JsonProperty("notes")]
	public string Notes { get; set; }

	[JsonProperty("transactionDate")]
	public DateTime TransactionDate { get; set; }

	[JsonProperty("bankName")]
	public string BankName { get; set; }

	[JsonProperty("accountName")]
	public string AccountName { get; set; }

	[JsonProperty("expenseType")]
	public string ExpenseType { get; set; }

	[JsonProperty("category")]
	public string Category { get; set; }

	[JsonProperty("subCategory")]
	public string SubCategory { get; set; }

	[JsonProperty("paymentMethod")]
	public string PaymentMethod { get; set; }

	[JsonProperty("lastModifiedUtc")]
	public DateTime LastModifiedUtc { get; set; }
}
