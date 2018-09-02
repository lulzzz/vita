using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Commands
{
    public class TextMatchBankStatement3CommandHandler  : CommandHandler<BankStatementAggregate, BankStatementId, TextMatchBankStatement3Command>
    {
        private readonly ITextClassifier _textClassifier;

        public TextMatchBankStatement3CommandHandler(ITextClassifier textClassifier)
        {
            _textClassifier = textClassifier;
        }

        public override async Task ExecuteAsync(BankStatementAggregate aggregate, TextMatchBankStatement3Command command,
            CancellationToken cancellationToken)
        {
            await aggregate.TextMatchAsync(command,_textClassifier);
        }
    }
}
