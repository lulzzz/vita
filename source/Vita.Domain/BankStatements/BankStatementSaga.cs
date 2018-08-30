﻿using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Sagas;
using EventFlow.Sagas.AggregateSagas;
using Vita.Domain.BankStatements.Commands;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
{
  public class BankStatementSaga : AggregateSaga<BankStatementSaga, BankStatementSagaId, BankStatementSagaLocator>,
    ISagaIsStartedBy<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event>
  {
    public BankStatementSaga(BankStatementSagaId id) : base(id)
    {
    }

    /// <summary>
    /// extract bank statements
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="sagaContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleAsync(
      IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event> domainEvent,
      ISagaContext sagaContext, CancellationToken cancellationToken)
    {
      Publish(new PredictBankStatement2Command());

      await Task.CompletedTask;
    }

    /// <summary>
    /// predict bank statement lines
    /// </summary>
    /// <param name="domainEvent"></param>
    /// <param name="sagaContext"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleAsync(
      IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent,
      ISagaContext sagaContext, CancellationToken cancellationToken)
    {
      Publish(new PredictBankStatement2Command());

      await Task.CompletedTask;
    }

    public async Task HandleAsync(
      IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent,
      ISagaContext sagaContext, CancellationToken cancellationToken)
    {
      Publish(new TextMatchBankStatement3Command());

      await Task.CompletedTask;
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
    var id = domainEvent.Metadata["aggregate_id"];
    var sagaId =
      new BankStatementSagaId($"BankStatementSagaId-{id.Replace("bankstatement-", string.Empty)}".ToLowerInvariant());

    return Task.FromResult<ISagaId>(sagaId);
  }
}