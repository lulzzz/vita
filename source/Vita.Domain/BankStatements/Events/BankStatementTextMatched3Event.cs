using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementTextMatched3Event :  IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
      public IEnumerable<PredictionResult> Unmatched { get; set; }
      public IList<KeyValuePair<PredictionResult, TextClassificationResult>> Matched { get; set; }
      
  }
}
