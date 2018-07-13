using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;

namespace Vita.Domain.Infrastructure
{
  public class DocumentDbRepository<T> : IDocumentDbRepository<T> where T : class
  {
    private readonly CosmosDbConfig _config;
    private readonly string _collectionId;

    public readonly DocumentClient Client;
    public readonly FeedOptions DefaultOptions = new FeedOptions
    {
      EnableCrossPartitionQuery = true,
      EnableScanInQuery = true,
      MaxItemCount = 5      
    };
    public Uri CollectionUri;


    public DocumentDbRepository(CosmosDbConfig config, string collectionId)
    {
      _config = config;
      _collectionId = collectionId;
      this.Client = new DocumentClient(new Uri(_config.Endpoint), _config.Key);
      CollectionUri = UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, collectionId);
      CreateDatabaseIfNotExistsAsync().Wait();
      CreateCollectionIfNotExistsAsync().Wait();
    }

    public async Task<T> GetItemAsync(string id)
    {
      try
      {
        Document document = await Client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_config.DatabaseId, _collectionId, id));
        return (T)(dynamic)document;
      }
      catch (DocumentClientException e)
      {
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          return null;
        }
        else
        {
          throw;
        }
      }
    }

    public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
    {
      IDocumentQuery<T> query = Client.CreateDocumentQuery<T>(
          UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _collectionId),
          new FeedOptions { MaxItemCount = -1 })
          .Where(predicate)
          .AsDocumentQuery();

      List<T> results = new List<T>();
      while (query.HasMoreResults)
      {
        results.AddRange(await query.ExecuteNextAsync<T>());
      }

      return results;
    }

    public bool Exists(string id)
    {
      var collectionUri = UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _collectionId);
      var query = Client.CreateDocumentQuery<Microsoft.Azure.Documents.Document>(collectionUri, new FeedOptions() { MaxItemCount = 1 });
      return query.Where(x => x.Id == id).Select(x => x.Id).AsEnumerable().Any(); //using Linq
    }

    public async Task<Document> CreateItemAsync(T item)
    {
      return await Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _collectionId), item);
    }

    public async Task<Document> UpdateItemAsync(string id, T item)
    {
      return await Client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_config.DatabaseId, _collectionId, id), item);
    }

    public async Task DeleteItemAsync(string id)
    {
      await Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_config.DatabaseId, _collectionId, id));
    }

    private async Task CreateDatabaseIfNotExistsAsync()
    {
      try
      {
        await Client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(_config.DatabaseId));
      }
      catch (DocumentClientException e)
      {
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          await Client.CreateDatabaseAsync(new Database { Id = _config.DatabaseId });
        }
        else
        {
          throw;
        }
      }
    }

    private async Task CreateCollectionIfNotExistsAsync()
    {
      try
      {
        await Client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(_config.DatabaseId, _collectionId));
      }
      catch (DocumentClientException e)
      {
        if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
          await Client.CreateDocumentCollectionAsync(
              UriFactory.CreateDatabaseUri(_config.DatabaseId),
              new DocumentCollection { Id = _collectionId },
              new RequestOptions { OfferThroughput = 1000 });
        }
        else
        {
          throw;
        }
      }
    }
  }
}