using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;

namespace Vita.Domain.BankStatements.Commands
{
  public class TextMatchBankStatement3Command : ICommand<BankStatementAggregate, BankStatementId, IExecutionResult>
  {
    public BankStatementId AggregateId { get; set; }

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
