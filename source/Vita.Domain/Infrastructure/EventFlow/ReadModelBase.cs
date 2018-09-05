using System;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public abstract class ReadModelBase
    {
        public string AggregateId { get; set; }

        public DateTimeOffset CreatedUtcDate { get; set; } = DateTimeOffset.UtcNow;
        public DateTimeOffset ModifiedUtcDate { get; set; } = DateTimeOffset.UtcNow;
        public int LastAggregateSequenceNumber { get; set; }
    }
}