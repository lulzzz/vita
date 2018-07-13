using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Azure.Documents;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Classifiers
{
  public class ClassifierHandler : CollectionBase, IConsumer<ClassifierRequest>
  {
    private readonly IRepository<Contracts.Classifier> _repository;

    public ClassifierHandler(IRepository<Contracts.Classifier> repository)
    {
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<ClassifierRequest> context)
    {
      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        LogVerbose($"received: {context.SentTime}");

        try
        {
          if (Export) await ExportToCosmosDb(context);

          _repository.Save(context.Message.Identifier);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Error(e, "error {err}", context.Message.Identifier.CategoryType);
          throw;
        }
      }

      await Task.Delay(TimeSpan.FromSeconds(1));
    }

    private static async Task ExportToCosmosDb(ConsumeContext<ClassifierRequest> context)
    {
      var config = new CosmosDbConfig();
      var repo = new DocumentDbRepository<Contracts.Classifier>(config,
        Constant.CosmosDbCollections.KeywordClassifiers);
      var query = repo.Client.CreateDocumentQuery<Contracts.Classifier>(repo.CollectionUri, new SqlQuerySpec
      {
        QueryText = @"SELECT * FROM KeywordClassifiers c WHERE CONTAINS(c.SubCategory, @SubCategory)",
        Parameters = new SqlParameterCollection
        {
          new SqlParameter("@SubCategory", context.Message.Identifier.SubCategory.ToUpperInvariant())
        }
      }, repo.DefaultOptions);

      if (context.Message.Identifier.Id == Guid.Empty)
      {
        await Insert(context, repo);
      }
      else
      {
        await Upsert(context, repo, query);
      }
    }

    private static async Task Upsert(ConsumeContext<ClassifierRequest> context, DocumentDbRepository<Contracts.Classifier> repo, IQueryable<Contracts.Classifier> query)
    {
      var exists = await repo.GetItemAsync(context.Message.Identifier.Id.ToString());

      if (!query.ToList().Any() && exists == null)

        if (!query.ToList().Any())
        {
          await Insert(context, repo);
        }
        else
        {
          Log.Debug("keywordclassifier update try {subcategory}", context.Message.Identifier.SubCategory);
          await repo.UpdateItemAsync(context.Message.Identifier.Id.ToString(),
            context.Message.Identifier);
          Log.Information("keywordclassifier update ok {subcategory}",
            context.Message.Identifier.SubCategory);
        }
    }

    private static async Task Insert(ConsumeContext<ClassifierRequest> context, DocumentDbRepository<Contracts.Classifier> repo)
    {
      Log.Debug("keywordclassifier insert try {subcategory}", context.Message.Identifier.SubCategory);
      await repo.CreateItemAsync(context.Message.Identifier);
      Log.Information("keywordclassifier insert ok {subcategory}",
        context.Message.Identifier.SubCategory);
    }
  }
}