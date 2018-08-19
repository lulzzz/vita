using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Companies.Events
{
    public class CompanyCreatedEvent :  EventBase<CompanyAggregate, CompanyId>
    {
        public Company Company { get; set; }
    }
}
