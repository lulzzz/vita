using System;

namespace Vita.DataImporter.Banks
{
    public class AnzModel
    {
      public DateTime TransactionDate { get; set; }
      public decimal Amount { get; set; }
      public string Description { get; set; }

      public override string ToString()
      {
        return $"{nameof(TransactionDate)}: {TransactionDate}, {nameof(Amount)}: {Amount}, {nameof(Description)}: {Description}";
      }
    }
}
