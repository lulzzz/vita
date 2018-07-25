using System;
using System.Collections.Generic;

namespace Vita.Contracts
{
  public class TextClassificationResult
  {

    public string SearchPhrase { get; set; }

    // who
    public Company Company { get; set; }
    public string BankCode { get; set; }
    public string BankName { get; set; }

    // what type of transaction
    public TransactionType? TransactionType { get; set; }

    // why - categories
    public Classifier Classifier { get; set; }

    // where
    public Locality Locality { get; set; }

    // when
    public DateTime? TransactionDate { get; set; }

    // how
    public PaymentMethodType? PaymentMethodType { get; set; }

    public IDictionary<int, IEnumerable<string>> Ngrams { get; set; } = new Dictionary<int, IEnumerable<string>>();

    public bool HasResult()
    {
      return Classifier != null;
    }
  }
}