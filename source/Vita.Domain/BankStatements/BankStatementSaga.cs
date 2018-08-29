using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
{
  public class BankStatementSaga : AggregateSaga<BankStatementSaga, BankStatementSagaId, BankStatementSagaLocator>,
    ISagaIsStartedBy<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event>
  {
    public BankStatementSaga(BankStatementSagaId id) : base(id)
    {
    }


    public Task HandleAsync(
      IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event> domainEvent,
      ISagaContext sagaContext, CancellationToken cancellationToken)
    {
      throw new NotImplementedException();
    }
  }
}

public class BankStatementSagaId : ISagaId
{
  public BankStatementSagaId(string value)
  {
    Value = value;
  }

  public string Value { get; }
}

public class BankStatementSagaLocator : ISagaLocator
{
  public Task<ISagaId> LocateSagaAsync(
    IDomainEvent domainEvent,
    CancellationToken cancellationToken)
  {
    var id = domainEvent.Metadata["bankstatementId"];
    var sagaId = new BankStatementSagaId($"bankstatementsagaId-{id}");

    return Task.FromResult<ISagaId>(sagaId);
  }
}