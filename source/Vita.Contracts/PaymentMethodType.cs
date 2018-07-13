using System.ComponentModel;

namespace Vita.Contracts
{
  public enum PaymentMethodType
  {
    [Description("Cash Withdrawl")] CashWithdrawl,
    [Description("Eftpos")] Eftpos,
    [Description("Direct Debit")] DirectDebit,
    [Description("Credit Card")] CreditCard,
    [Description("Unknown")] Unknown
  }
}