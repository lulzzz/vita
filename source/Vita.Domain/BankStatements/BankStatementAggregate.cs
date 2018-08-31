using System.Threading.Tasks;
using EventFlow.Aggregates;
using Vita.Domain.BankStatements.Commands;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
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
            //TODO extract bank statements
            Emit(new BankStatementExtracted1Event(){});
            await Task.CompletedTask;
        }

      public async Task PredictAsync(PredictBankStatement2Command command)
      {
        //TODO predict each transaction
        Emit(new BankStatementPredicted2Event(){});
        await Task.CompletedTask;
      }

      public async Task TextMatchAsync(TextMatchBankStatement3Command command)
      {
        //TODO text match unclassified each transaction
        Emit(new BankStatementTextMatched3Event(){});
        await Task.CompletedTask;

      }
    }
}