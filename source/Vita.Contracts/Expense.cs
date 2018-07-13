namespace Vita.Contracts
{
    public class Expense : ValueObject
    {
        public ExpenseType ExpenseType { get; }
        public decimal Amount { get; }
        public Vita.Contracts.Frequency Frequency { get; }

        public Expense(ExpenseType expenseType, decimal amount, Frequency frequency =  Frequency.Weekly)
        {
            ExpenseType = expenseType;
            Amount = amount;
            Frequency = frequency;
        }
    }
}