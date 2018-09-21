using System.Collections.Generic;

namespace Vita.Contracts
{
  public class BankStatementAnalysisSummaryView
  {
    public IDictionary<string, decimal> CategoryTotals{ get; set; }

    public IDictionary<string, decimal> SubCategoryTotals{ get; set; }

    public IEnumerable<string> Unmatched { get; set; }
    public string BankStatementId { get; set; }
  }
}