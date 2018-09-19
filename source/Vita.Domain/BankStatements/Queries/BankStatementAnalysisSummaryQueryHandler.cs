using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.MsSql;
using EventFlow.Queries;
using EventFlow.ReadStores;
using ExtensionMinder;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.BankStatements.ReadModels;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.BankStatements.Queries
{
  public class
    BankStatementAnalysisSummaryQueryHandler : IQueryHandler<BankStatementAnalysisSummaryQuery,
      BankStatementAnalysisSummaryView>
  {
    private readonly IEventStore _eventStore;
    private readonly IAggregateStore _aggregateStore;
    private readonly IReadModelStore<BankStatementReadModel> _readModelStore;
    private readonly IMsSqlConnection _msSqlConnection;

    public BankStatementAnalysisSummaryQueryHandler(IEventStore eventStore, IAggregateStore aggregateStore,
      IReadModelStore<BankStatementReadModel> readModelStore, IMsSqlConnection msSqlConnection)
    {
      _eventStore = eventStore;
      _aggregateStore = aggregateStore;
      _readModelStore = readModelStore;
      _msSqlConnection = msSqlConnection;
    }

    public async Task<BankStatementAnalysisSummaryView> ExecuteQueryAsync(BankStatementAnalysisSummaryQuery query,
      CancellationToken cancellationToken)
    {
      Logger.Debug($"query: ", query.BankStatementId);
      //    var aggregate = await _aggregateStore.LoadAsync<BankStatementAggregate, BankStatementId>(query.BankStatementId,cancellationToken);

      var readModels = await _msSqlConnection.QueryAsync<BankStatementReadModel>(
          Label.Named("mssql-fetch-BankStatementReadModel"),
          cancellationToken,
          "SELECT * FROM [BankStatementReadModel] WHERE AggregateId = @AggregateId",
          new {AggregateId = query.BankStatementId.Value})
        .ConfigureAwait(false);

      var converted = readModels
       //.Where(x => x.TransactionUtcDate > query.FromUtcDateTime && x.TransactionUtcDate < query.ToUtcDateTime)
        .ToList()
        .Select(x => new {x.Category, x.SubCategory, x.Amount})
        .ToList();

      Logger.Debug($"query results: {converted.Count}");


      var cats = from p in converted
        group p by p.Category
        into g
        select new {Category = g.Key, Total = converted.Where(a => a.Category == g.Key)
          .Sum(x => x.Amount)};

      var subs = from p in converted.Where(x=>!string.IsNullOrEmpty(x.SubCategory))
        group p by p.SubCategory
        into g
        select new {SubCategory = g.Key, Total = converted.Where(a => a.SubCategory == g.Key)
          .Sum(x => x.Amount)};

      var view = new BankStatementAnalysisSummaryView
      {
        BankStatementId = query.BankStatementId.Value,
        CategoryTotals = new ConcurrentDictionary<CategoryType, decimal>(),
        SubCategoryTotals = new ConcurrentDictionary<string, decimal>(),
        Unmatched = readModels.Where(x=>x.SubCategory == Categories.Uncategorised).Select(x=>x.Description).ToList()
      };

      cats.AsParallel().ForAll(x => view.CategoryTotals.Add(new KeyValuePair<CategoryType, decimal>(x.Category, x.Total)));
      subs.AsParallel().ForAll((x => view.SubCategoryTotals.Add(new KeyValuePair<string, decimal>(x.SubCategory, x.Total))));

      return view;
    }
  }
}