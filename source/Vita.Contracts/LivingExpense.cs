namespace Vita.Contracts
{
    public class LivingExpense : Expense
    {
        public LivingExpense(decimal amount) 
            : base(ExpenseType.Living, amount)
        {
            
        }
    }
}