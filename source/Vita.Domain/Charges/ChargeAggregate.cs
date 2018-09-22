using System.Collections.Generic;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using Vita.Domain.Charges.Commands;

namespace Vita.Domain.Charges
{
    [AggregateName("Charge")]
    public class ChargeAggregate : AggregateRoot<ChargeAggregate, ChargeId>
    {
        public ChargeState State { get; protected set; }

        public ChargeAggregate(ChargeId id) : base(id)
        {
            State = new ChargeState();
            Register(State);
        }

        public virtual async Task ImportCharges(IEnumerable<ImportChargesCommandHandler.ImportedCharge> data)
        {
            await Task.CompletedTask;
        }
    }
}