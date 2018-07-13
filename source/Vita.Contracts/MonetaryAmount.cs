using System.Globalization;

namespace Vita.Contracts
{
    public class MonetaryAmount  
    {
        private static readonly CultureInfo Culture = new CultureInfo("en-AU");

        public decimal Amount { get; set; }

        public MonetaryAmount(decimal amount)  
        {
            Amount = amount;
        }

        public override string ToString()
        {
            return Amount.ToString(Culture);
        }
    }
}