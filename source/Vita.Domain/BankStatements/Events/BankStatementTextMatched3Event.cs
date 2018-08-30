using EventFlow.Aggregates;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementTextMatched3Event :  IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
  }
}
