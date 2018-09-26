using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Domain.Charges.Commands;

namespace Vita.Domain.Charges.Events
{
    public class ChargesImportedEvent  : IAggregateEvent<ChargeAggregate, ChargeId>
    {
        public IEnumerable<ImportChargesCommandHandler.ImportedCharge> ImportedCharges { get; set; }
    }
}
