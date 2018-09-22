using EventFlow.Core;

namespace Vita.Domain.Charges
{
    public class ChargeId: Identity<ChargeId>
    {
        public ChargeId(string value) : base(value)
        {
        }
    }
}