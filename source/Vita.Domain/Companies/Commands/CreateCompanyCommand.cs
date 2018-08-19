using EventFlow.Commands;
using EventFlow.Core;
using Vita.Contracts;

namespace Vita.Domain.Companies.Commands
{
    public class CreateCompanyCommand : Command<CompanyAggregate, CompanyId>
    {
        public Company Company { get; set; }

        public CreateCompanyCommand(CompanyId aggregateId) : base(aggregateId)
        {
        }

        public CreateCompanyCommand(CompanyId aggregateId, ISourceId sourceId) : base(aggregateId, sourceId)
        {
        }
    }
}
