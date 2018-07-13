using System;
using System.Linq;
using System.Threading.Tasks;
using FizzWare.NBuilder;
using Microsoft.Azure.Documents;
using Vita.Contracts;
using Vita.Domain.Infrastructure;
using Xunit;

namespace Vita.Domain.Tests.Infrastructure
{
  public class DocumentDbRepositoryShould
  {
    [Fact]
    public async Task Retrieve_companies_from_azure()
    {
      var repo = new DocumentDbRepository<Company>(new CosmosDbConfig(), Constant.CosmosDbCollections.Companies);
      var company = Builder<Company>
        .CreateNew()
        .With(x=>x.Id, Guid.NewGuid())
        .With(x=>x.CompanyName, "ZZZZZZZZZZ")
        .Build();
      var item = await repo.CreateItemAsync(company);
    

      var query = repo.Client.CreateDocumentQuery<Company>(repo.CollectionUri, new SqlQuerySpec
      {
        QueryText = @"SELECT * FROM Companies c WHERE CONTAINS(c.CompanyName, @CompanyName)",
        Parameters = new SqlParameterCollection
        {
          new SqlParameter("@CompanyName", company.CompanyName.ToUpperInvariant())
        }
      }, repo.DefaultOptions);


      var result = query.ToList().Single();
      Assert.Equal(company.CompanyName, result.CompanyName);
      Assert.Equal(company.Status, result.Status);

      await repo.DeleteItemAsync(item.Id);
      //var result2 = await repo.GetItemAsync(item.Id);
      //Assert.Null(result2);
    }
  }
}