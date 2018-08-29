using EventFlow.Aggregates;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementExtractedEvent : IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
  }
}
