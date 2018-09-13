using System;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Azure.Documents;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Companies.Old
{
  public class CompanyHandler : CollectionBase, IConsumer<CompanyRequest>
  {
    private IBusControl _bus;
    private readonly IRepository<Company> _repository;

    public CompanyHandler(IBusControl bus, IRepository<Company> repository)
    {
      _bus = bus;
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<CompanyRequest> context)
    {
      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        Log.Verbose($"AsicCompanyRequest received: {context.SentTime}");

        try
        {
          if (Export) await ExportToCosmosDb(context);

          _repository.Save(context.Message.Company);
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Error(e, "error {err}", context.Message.Company.CompanyName);
          throw;
        }
      }

      await Task.Delay(TimeSpan.FromSeconds(1));
    }

    private static async Task ExportToCosmosDb(ConsumeContext<CompanyRequest> context)
    {
      var config = new CosmosDbConfig();
      var repo = new DocumentDbRepository<Company>(config, Constant.CosmosDbCollections.Companies);
      var query = repo.Client.CreateDocumentQuery<Company>(repo.CollectionUri, new SqlQuerySpec
      {
        QueryText = @"SELECT * FROM Companies c WHERE CONTAINS(c.CompanyName, @CompanyName)",
        Parameters = new SqlParameterCollection
        {
          new SqlParameter("@CompanyName", context.Message.Company.CompanyName.ToUpperInvariant())
        }
      }, repo.DefaultOptions);


      var exists = await repo.GetItemAsync(context.Message.Company.Id.ToString());

      if (!query.ToList().Any() && exists == null)
      {
        Log.Debug("company insert try {company}", context.Message.Company.CompanyName);
        await repo.CreateItemAsync(context.Message.Company);
        Log.Information("company insert ok {company}", context.Message.Company.CompanyName);
      }
      else
      {
        Log.Debug("company update try {searchphrase}", context.Message.Company.CompanyName);
        await repo.UpdateItemAsync(context.Message.Company.Id.ToString(), context.Message.Company);
        Log.Information("company update ok {searchphrase}", context.Message.Company);
      }
    }
  }
}