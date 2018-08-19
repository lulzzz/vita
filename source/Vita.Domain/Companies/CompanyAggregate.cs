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
        public CompanyState State { get; private set; }

        public CompanyAggregate(CompanyId id) : base(id)
        {
            State = new CompanyState();
            Register(State);
        }

        public async Task<CompanyExecutionResult> CreateCompanyAsync(CreateCompanyCommand command)
        {
            if (!IsNew)
            {
                throw DomainError.With("company exists");
            }

            Emit(new CompanyCreatedEvent(){Company = command.Company});
            return await Task.FromResult(new CompanyExecutionResult() {IsSuccess = true});
        }
    }
}
