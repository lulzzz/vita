using EventFlow.Aggregates;
using EventFlow.Core;

namespace Vita.Domain.Infrastructure
{
    public abstract class EventBase<T, TId> : AggregateEvent<T, TId> where T : IAggregateRoot<TId> where TId : IIdentity
    {
    }
}
