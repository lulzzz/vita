using EventFlow.Aggregates;

namespace Vita.Domain.BankStatements
{
  public class BankStatementState :  AggregateState<BankStatementAggregate, BankStatementId, BankStatementState>
  {
  }
}
