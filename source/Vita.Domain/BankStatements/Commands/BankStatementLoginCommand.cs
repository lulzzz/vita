using EventFlow.Commands;
using EventFlow.Core;

namespace Vita.Domain.BankStatements.Commands
{
    public class BankStatementLoginCommand : Command<BankStatementAggregate, BankStatementId>
    {
        public BankStatementLoginCommand(BankStatementId aggregateId) : base(aggregateId)
        {
        }

        public BankStatementLoginCommand(BankStatementId aggregateId, ISourceId sourceId) : base(aggregateId, sourceId)
        {
        }
    }
}
