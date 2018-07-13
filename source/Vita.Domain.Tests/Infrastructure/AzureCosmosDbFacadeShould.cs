using System.Threading.Tasks;
using Xunit;

namespace Vita.Domain.Tests.Infrastructure
{
  public class AzureCosmosDbFacadeShould
  {
    [Fact]
    public void AzureCosmosDb_searchdocument_insert()
    {
      //var cosmosDb = new AzureCosmosDbFacade();

      //var collection = cosmosDb.GetDatabase().GetCollection<SearchDocument>("SearchData");

      //var data = new SearchDocument
      //{
      //  Id = Guid.NewGuid(),
      //  SearchText = "search text test",
      //  Category1 = PlaceLocationType.Uknown,
      //  CompanyId = Guid.NewGuid(),
      //  ChargeType = TransactionType.Credit
      //};

      //collection.FindOneAndDelete<SearchDocument>(
      //  new ExpressionFilterDefinition<SearchDocument>(x => x.SearchText == data.SearchText));

      //collection.InsertOne(data);

      //var found = collection.FindSync<SearchDocument>(
      //  new ExpressionFilterDefinition<SearchDocument>(x => x.Id == data.Id));
      //if (found.MoveNext())
      //  Assert.Equal(found.Current.First().CompanyId, data.CompanyId);
      //else
      //  Assert.False(true);

      //collection.FindOneAndDelete(x => x.Id == data.Id);
    }

    [Fact]
    public void AzureCosmosDb_company_insert()
    {
      //var cosmosDb = new AzureCosmosDbFacade();

      //var collection = cosmosDb.GetDatabase().GetCollection<Company>("Companies");

      //var data = Builder<Company>.CreateNew().Build();

      //collection.FindOneAndDelete<Company>(
      //  new ExpressionFilterDefinition<Company>(x => x.CompanyName == data.CompanyName));

      //collection.InsertOne(data);

      //var found = collection.FindSync<Company>(new ExpressionFilterDefinition<Company>(x => x.Id == data.Id));
      //if (found.MoveNext())
      //  Assert.Equal(found.Current.First().CompanyName, data.CompanyName);
      //else
      //  Assert.False(true);

      //collection.FindOneAndDelete(x => x.Id == data.Id);
    }

    [Fact]
    public async Task AzureCosmosDb_company_find()
    {
      //const string xxx = @"EXCEL TRAINING SOLUTIONS AUSTRALIA PTY LTD                                                          ";
      //var cosmosDb = new AzureCosmosDbFacade();
      //var collection = cosmosDb.GetDatabase().GetCollection<Company>("Companies");

      //var cursor = await collection.FindAsync(
      //  new ExpressionFilterDefinition<Company>(x => x.CompanyName == xxx));

      //bool exists = await cursor.AnyAsync();

      //Assert.True(exists);

    }
  }
}