using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.Core;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementTextMatched3Event :  IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
      public IEnumerable<PredictionResult> Unmatched { get; set; }
      public List<Tuple<PredictionResult, TextClassificationResult>> Matched { get; set; }
  }
}
