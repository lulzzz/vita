using EventFlow.Aggregates;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
{
  public class BankStatementState : AggregateState<BankStatementAggregate, BankStatementId, BankStatementState>,
    IEmit<BankStatementExtracted1Event>,
    IEmit<BankStatementPredicted2Event>,
    IEmit<BankStatementTextMatched3Event>
  {
    public void Apply(BankStatementExtracted1Event aggregateEvent)
    {
    }

    public void Apply(BankStatementPredicted2Event aggregateEvent)
    {
    }

    public void Apply(BankStatementTextMatched3Event aggregateEvent)
    {
    }
  }
}