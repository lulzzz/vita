namespace Vita.Contracts
{
    public class LoanRequest : ValueObject
    {
        public decimal Amount { get; set; }
        public LoanReasonType ReasonType { get; set; }
        public Agreement Agreement { get; set; }

        public LoanRequest(decimal amount, LoanReasonType reasonType, Agreement agreement)
        {
            Amount = amount;
            ReasonType = reasonType;
            Agreement = agreement;
        }
    }
}