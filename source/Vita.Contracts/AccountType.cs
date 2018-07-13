using System.ComponentModel;

namespace Vita.Contracts
{
  public enum AccountType
  {
    [Description("Credit")] Credit,
    [Description("Debit")] Debit,
    [Description("Offset")] Offset,
    [Description("Facility")] Facility
  }

  public static class AccountTypeConverter
  {
    public static AccountType? Convert(string text)
    {
      if (string.IsNullOrWhiteSpace(text)) return  null;

      text = text.ToLowerInvariant();

      switch (text)
      {
        case "saving":
        case "debit":
        case "transaction":
        case "everyday":
          return AccountType.Debit;
        case "credit":
        case "credit card":
          return AccountType.Credit;
      }

      if (text.Contains("credit")) return AccountType.Credit;

      if (text.Contains("debit")) return AccountType.Debit;

      if (text.Contains("facility")) return AccountType.Facility;

      if (text.Contains("mortgage")) return AccountType.Offset;

      if (text.Contains("offset")) return AccountType.Offset;

      return null;
    }
  }
}