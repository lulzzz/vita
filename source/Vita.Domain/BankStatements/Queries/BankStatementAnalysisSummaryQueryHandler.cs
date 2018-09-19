using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow.EventStores;
using EventFlow.MsSql;
using EventFlow.Queries;
using EventFlow.ReadStores;
using Vita.Contracts;
using Vita.Domain.BankStatements.ReadModels;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.BankStatements.Queries
{
    public class BankStatementAnalysisSummaryQueryHandler : IQueryHandler<BankStatementAnalysisSummaryQuery, BankStatementAnalysisSummaryView>
    {
        private readonly IEventStore _eventStore;
        private readonly IAggregateStore _aggregateStore;
        private readonly IReadModelStore<BankStatementReadModel> _readModelStore;
        private readonly IMsSqlConnection _msSqlConnection;

        public BankStatementAnalysisSummaryQueryHandler(IEventStore eventStore, IAggregateStore aggregateStore, IReadModelStore<BankStatementReadModel> readModelStore,IMsSqlConnection msSqlConnection)
        {
            _eventStore = eventStore;
            _aggregateStore = aggregateStore;
            _readModelStore = readModelStore;
            _msSqlConnection = msSqlConnection;
        }

        public async Task<BankStatementAnalysisSummaryView> ExecuteQueryAsync(BankStatementAnalysisSummaryQuery query, CancellationToken cancellationToken)
        {
            Logger.Debug($"query: ", query.BankStatementId);
            var aggregate = await _aggregateStore.LoadAsync<BankStatementAggregate, BankStatementId>(query.BankStatementId,cancellationToken);

            var readModels = await _msSqlConnection.QueryAsync<BankStatementReadModel>(
                    Label.Named("mssql-fetch-BankStatementReadModel"),
                    cancellationToken,
                    "SELECT * FROM [BankStatementReadModel] WHERE AggregateId = @AggregateId",
                    new { AggregateId = query.BankStatementId.Value })
                .ConfigureAwait(false);

            var data = readModels.Select(x => new {x.Description,x.Amount});

            return new BankStatementAnalysisSummaryView();
         
        }
    }
}
