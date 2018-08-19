using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Domain.Companies.Events;

namespace Vita.Domain.Companies
{
    public class CompanyState : AggregateState<CompanyAggregate, CompanyId, CompanyState>,
        IEmit<CompanyCreatedEvent>
    {
        public IList<string> Companies { get; set; }

        public void Apply(CompanyCreatedEvent aggregateEvent)
        {
            if (Companies==null) Companies = new List<string>();

            Companies.Add(aggregateEvent.Company.CompanyName);
        }
    }
}
