using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Azure.Documents;
using Serilog.Context;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Charges
{
  public class ChargeSeedHandler : CollectionBase, IConsumer<ChargeSeedRequest>
  {
    private readonly ITextClassifier _textClassifier;

    public ChargeSeedHandler(ITextClassifier textClassifier)
    {
      _textClassifier = textClassifier;
    }

    public async Task Consume(ConsumeContext<ChargeSeedRequest> context)
    {

      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        LogVerbose($"received: {context.SentTime}");

        try
        {
          var config = new CosmosDbConfig();
          var repo = new DocumentDbRepository<Charge>(config, Constant.CosmosDbCollections.Charges);
          var query = repo.Client.CreateDocumentQuery<Charge>(repo.CollectionUri, new SqlQuerySpec
          {
            QueryText = @"SELECT * FROM Charges c WHERE CONTAINS(c.SearchPhrase, @SearchPhrase)",
            Parameters = new SqlParameterCollection
            {
              new SqlParameter("@SearchPhrase", context.Message.Charge.SearchPhrase)
              //new SqlParameter("@Id", context.Message.Charge.Id)
            }
          }, repo.DefaultOptions);

          var exists = await repo.GetItemAsync(context.Message.Charge.Id.ToString());

          if (!query.ToList().Any() && exists == null)
          {
            LogDebug($"insert try {context.Message.Charge.SearchPhrase}");
            await repo.CreateItemAsync(context.Message.Charge);
            LogInfo($"insert ok {context.Message.Charge.SearchPhrase}");
          }
          else
          {
            LogDebug($"update try {context.Message.Charge.SearchPhrase}");
            await repo.UpdateItemAsync(context.Message.Charge.Id.ToString(), context.Message.Charge);
            LogInfo("update ok {context.Message.Charge.SearchPhrase}");
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          LogError(e, "{context.Message.Charge.SearchPhrase}");
         // throw;
        }
      }

      await Task.Delay(TimeSpan.FromSeconds(3));
    }

 
  }
}