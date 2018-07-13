namespace Vita.Contracts
{
	public class Rent : ValueObject
	{
	    public decimal Amount { get;  }
	    public Frequency Frequency { get; }

		public Rent(decimal amount, Frequency frequency)
		{
		    Amount = amount;
		    Frequency = frequency;
		}
	}
}