using EventFlow.Aggregates;
using EventFlow.Core;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public class StatelessAggregateRoot<T, TId> : AggregateRoot<T, TId> where T : AggregateRoot<T, TId> where TId : IIdentity
    {

        public StatelessAggregateRoot(TId id) : base(id)
        {
        }

        /// <summary>
        /// Stateless so do nothing
        /// </summary>
        protected override void ApplyEvent(IAggregateEvent<T, TId> aggregateEvent)
        {
            Version++;
        }
    }
}