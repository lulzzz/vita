using System.ComponentModel;

namespace Vita.Contracts
{
  public enum TransactionType
  {
    [Description("Credit")] Credit = 1,
    [Description("Debit")] Debit = 2,
    [Description("Transfer")] Transfer = 3,
    [Description("Reversal")] Reversal = 4,
    [Description("Dishonour")] Dishonour = 5,
    [Description("Fees")] Fees = 6,
    [Description("Overdrawn")] Overdrawn = 7,
    [Description("Interest")] Interest = 8,
    [Description("Repayments")] Repayments = 9,
    [Description("Unknown")] Unknown = 10,
  }
}