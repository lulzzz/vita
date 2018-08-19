using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;
using Vita.Contracts;

namespace Vita.Domain.Companies.Commands
{
    public class CreateCompanyCommand : ICommand<CompanyAggregate, CompanyId, IExecutionResult>
    {
        public Company Company { get; set; }

        public CreateCompanyCommand( CompanyId aggregateId)
        {
            SourceId = this.GetSourceId();
            AggregateId = aggregateId;
        }

        public CreateCompanyCommand(ISourceId sourceId, CompanyId aggregateId)
        {
            SourceId = sourceId;
            AggregateId = aggregateId;
        }
 
        public async Task<IExecutionResult> PublishAsync(ICommandBus commandBus, CancellationToken cancellationToken)
        {
            var result =
                await commandBus.PublishAsync(this,cancellationToken);
            return new CompanyExecutionResult(){IsSuccess = result.IsSuccess};
        }

        public ISourceId GetSourceId()
        {
            return new SourceId(Guid.NewGuid().ToString());
        }

        public CompanyId AggregateId { get; }
        public ISourceId SourceId { get; }
    }
}
