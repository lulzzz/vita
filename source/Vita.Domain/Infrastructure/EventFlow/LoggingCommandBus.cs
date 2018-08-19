using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;
using Serilog.Context;
using Serilog.Core;
using Serilog.Events;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public class LoggingCommandBus : ICommandBus
    {
        private readonly ICommandBus _inner;

        public LoggingCommandBus(ICommandBus inner)
        {
            _inner = inner;
        }

        public Task<TExecutionResult> PublishAsync<TAggregate, TIdentity, TExecutionResult>(ICommand<TAggregate, TIdentity, TExecutionResult> command, CancellationToken cancellationToken) where TAggregate : IAggregateRoot<TIdentity> where TIdentity : IIdentity where TExecutionResult : IExecutionResult
        {
            using (LogContext.PushProperty("AggregateId", command.AggregateId))
            using (LogContext.PushProperty("Command", command.GetType().Name))
            {
                return _inner.PublishAsync(command, cancellationToken);
            }
        }
    }
}
