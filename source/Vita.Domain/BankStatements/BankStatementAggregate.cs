using EventFlow.Aggregates;

namespace Vita.Domain.BankStatements
{
    [AggregateName("BankStatement")]
    public class BankStatementAggregate : AggregateRoot<BankStatementAggregate, BankStatementId>
    {
        public BankStatementAggregate(BankStatementId id) : base(id)
        {
        }
    }
}
