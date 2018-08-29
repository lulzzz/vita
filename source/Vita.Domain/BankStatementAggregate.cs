using System.Threading.Tasks;
using EventFlow.Aggregates;
using Vita.Domain.BankStatements;
using Vita.Domain.BankStatements.Commands;
using Vita.Domain.BankStatements.Events;


namespace Vita.Domain
{
    [AggregateName("BankStatement")]
    public class BankStatementAggregate : AggregateRoot<BankStatementAggregate, BankStatementId>
    {
        public BankStatementState State { get; protected set; }

        public BankStatementAggregate(BankStatementId id) : base(id)
        {
            State = new BankStatementState();
            Register(State);
        }

        public async Task ExtractBankStatementAsync(ExtractBankStatement1Command command)
        {
            Emit(new BankStatementExtracted1Event(){});
            await Task.CompletedTask;
        }
    }
}