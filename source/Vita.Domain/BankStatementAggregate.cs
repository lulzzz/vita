using EventFlow.Aggregates;
using Vita.Domain.BankStatements;

namespace Vita.Domain
{
  [AggregateName("BankStatement")]
  public class BankStatementAggregate : AggregateRoot<BankStatementAggregate, BankStatementId>
  {
    public BankStatementState State { get; private set; }
    public BankStatementAggregate(BankStatementId id) : base(id)
    {
      State = new BankStatementState();
      Register(State);
    }

   
  }
}
