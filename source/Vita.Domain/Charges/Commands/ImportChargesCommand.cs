using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using EventFlow.Core;
using Newtonsoft.Json;

namespace Vita.Domain.Charges.Commands
{
    public class ImportChargesCommand  : Command<ChargeAggregate, ChargeId, IExecutionResult>
    {
        public ImportChargesCommand(ChargeId aggregateId) : base(aggregateId)
        {
        }

        [JsonConstructor]
        public ImportChargesCommand(ChargeId aggregateId, ISourceId sourceId) : base(aggregateId, sourceId)
        {
        }
    }
}
