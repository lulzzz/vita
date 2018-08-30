using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Vita.Domain.BankStatements.Commands
{
    public class TextMatchBankStatement3CommandHandler  : CommandHandler<BankStatementAggregate, BankStatementId, ExtractBankStatement1Command>
    {
        public override async Task ExecuteAsync(BankStatementAggregate aggregate, ExtractBankStatement1Command command,
            CancellationToken cancellationToken)
        {
            await aggregate.TextMatchAsync(command);
        }
    }
}
