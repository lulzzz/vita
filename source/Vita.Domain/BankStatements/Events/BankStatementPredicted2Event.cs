using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementPredicted2Event :  IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
      public IEnumerable<PredictionResult> PredictionResults { get; set; }
  }
}
