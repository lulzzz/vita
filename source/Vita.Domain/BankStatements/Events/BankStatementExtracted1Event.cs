using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Events
{
  public class BankStatementExtracted1Event : IAggregateEvent<BankStatementAggregate, BankStatementId>
  {
      public IEnumerable<PredictionRequest> PredictionRequests { get; set; }
  }
}
