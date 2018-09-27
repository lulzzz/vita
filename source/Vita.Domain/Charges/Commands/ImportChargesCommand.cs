using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;
using Vita.Domain.BankStatements;

namespace Vita.Domain.Charges.Commands
{
  public class ImportChargesCommand  : ICommand<ChargeAggregate, ChargeId, IExecutionResult>
  {
    public ChargeId AggregateId { get; set; }

    public ISourceId SourceId { get; set; }

    public ISourceId GetSourceId()
    {
      return new SourceId(Guid.NewGuid().ToString());
    }

    public Task<IExecutionResult> PublishAsync(ICommandBus commandBus, CancellationToken cancellationToken)
    {
      var result = ExecutionResult.Failed("not run from here");
      return Task.FromResult(result);
    }
  }
}
