using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Vita.Domain.BankStatements.Commands
{
    public class PredictBankStatement2CommandHandler  : CommandHandler<BankStatementAggregate, BankStatementId, PredictBankStatement2Command>
    {
        public override async Task ExecuteAsync(BankStatementAggregate aggregate, PredictBankStatement2Command command,
            CancellationToken cancellationToken)
        {
          await aggregate.PredictAsync(command);
        }
    }
}
