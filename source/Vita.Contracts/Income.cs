namespace Vita.Contracts
{
	public class Income : ValueObject
	{
        public IncomeType IncomeType { get; set; }

	    public Frequency Frequency { get; set; }

	    public decimal Amount { get; set; }

	    public Income(IncomeType incomeType, Frequency frequency, decimal amount)
	    {
            IncomeType = incomeType;
            Frequency = frequency;
	        Amount = amount;
	    }
	}
}