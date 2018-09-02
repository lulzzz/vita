using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Commands
{
    public class PredictBankStatement2CommandHandler  : CommandHandler<BankStatementAggregate, BankStatementId, PredictBankStatement2Command>
    {
        private readonly IPredict _predict;

        public PredictBankStatement2CommandHandler(IPredict predict)
        {
            _predict = predict;
        }

        public override async Task ExecuteAsync(BankStatementAggregate aggregate, PredictBankStatement2Command command,
            CancellationToken cancellationToken)
        {
          await aggregate.PredictAsync(command,_predict);
        }
    }
}
