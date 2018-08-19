using EventFlow.Core;

namespace Vita.Domain.Companies
{
    public class CompanyId : Identity<CompanyId>
    {
        public CompanyId(string value) : base(value)
        {
        }
    }
}