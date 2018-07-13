using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Azure.Documents;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Localities
{
  public class KeywordIdentifierHandler : CollectionBase, IConsumer<LocalityRequest>
  {
    private readonly IRepository<Locality> _repository;

    public KeywordIdentifierHandler(IRepository<Locality> repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<LocalityRequest> context)
    {
      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        LogVerbose($"received: {context.SentTime}");

        try
        {          
          if (Export) await ExportToCosmosDb(context);

          _repository.Save(context.Message.Locality);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Error(e, "error {err}", context.Message.Locality.Suburb);
          throw;
        }
      }

      await Task.Delay(TimeSpan.FromMilliseconds(10));
    }

    private static async Task ExportToCosmosDb(ConsumeContext<LocalityRequest> context)
    {
      var config = new CosmosDbConfig();
      var repo = new DocumentDbRepository<Locality>(config, Constant.CosmosDbCollections.Localities);
      var query = repo.Client.CreateDocumentQuery<Locality>(repo.CollectionUri, new SqlQuerySpec
      {
        QueryText = @"SELECT * FROM Localities c WHERE CONTAINS(c.SuburbName, @SuburbName)",
        Parameters = new SqlParameterCollection
        {
          new SqlParameter("@SuburbName", context.Message.Locality.Suburb.ToUpperInvariant())
        }
      }, repo.DefaultOptions);


      var exists = await repo.GetItemAsync(context.Message.Locality.Id.ToString());

      if (!query.ToList().Any() && exists == null)
      {
        Log.Debug("locality insert try {suburb}", context.Message.Locality.Suburb);
        await repo.CreateItemAsync(context.Message.Locality);
        Log.Information("locality insert ok {suburb}", context.Message.Locality.Suburb);
      }
      else
      {
        Log.Debug("locality insert try {suburb}", context.Message.Locality.Suburb);
        await repo.UpdateItemAsync(context.Message.Locality.Id.ToString(), context.Message.Locality);
        Log.Information("locality update ok {suburb}", context.Message.Locality.Suburb);
      }
    }
  }
}