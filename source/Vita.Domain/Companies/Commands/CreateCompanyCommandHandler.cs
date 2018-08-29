using System.Threading;
using System.Threading.Tasks;
using EventFlow.Commands;

namespace Vita.Domain.Companies.Commands
{
    public class CreateCompanyCommandHandler : CommandHandler<CompanyAggregate, CompanyId, CreateCompanyCommand>
    {
        public override async Task ExecuteAsync(CompanyAggregate aggregate, CreateCompanyCommand command, CancellationToken cancellationToken)
        {
            var result = await aggregate.CreateCompanyAsync(command);
        }
    }
}
