using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Exceptions;
using Vita.Domain.Companies.Commands;
using Vita.Domain.Companies.Events;

namespace Vita.Domain.Companies
{
    [AggregateName("Company")]
    public class CompanyAggregate  : AggregateRoot<CompanyAggregate, CompanyId>
    {
        public CompanyAggregate(CompanyId id) : base(id)
        {
        }

        public async Task CreateCompanyAsync(CreateCompanyCommand command)
        {
            if (!IsNew)
            {
                throw DomainError.With("company exists");
            }

            await Task.CompletedTask;
            Emit(new CompanyCreatedEvent());
        }
    }
}
