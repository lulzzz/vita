using EventFlow.Aggregates;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementExtracted1Event : IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
  }
}
