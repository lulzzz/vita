using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Domain.Charges.Commands;
using Vita.Domain.Charges.Events;

namespace Vita.Domain.Charges
{
    public class ChargeState : AggregateState<ChargeAggregate, ChargeId, ChargeState>,IEmit<ChargesImportedEvent>
    {
        public void Apply(ChargesImportedEvent aggregateEvent)
        {
            ImportedCharges = aggregateEvent.ImportedCharges;
        }

        public IEnumerable<ImportChargesCommandHandler.ImportedCharge> ImportedCharges { get; set; }
    }
}