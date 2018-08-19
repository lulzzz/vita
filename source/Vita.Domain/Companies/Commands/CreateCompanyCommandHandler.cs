using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Vita.Domain.Companies.Commands
{
    public class CreateCompanyCommandHandler  : EventFlow.Commands.CommandHandler<CompanyAggregate, CompanyId, CreateCompanyCommand>
    {
        public override async Task ExecuteAsync(CompanyAggregate aggregate, CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            await aggregate.CreateCompanyAsync(command);
        }
    }
}
