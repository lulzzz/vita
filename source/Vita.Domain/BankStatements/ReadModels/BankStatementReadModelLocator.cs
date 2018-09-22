using System.Collections.Generic;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements.ReadModels
{
  /// <summary>
  ///   https://github.com/eventflow/EventFlow/issues/502
  /// </summary>
  public class BankStatementReadModelLocator : IReadModelLocator
  {
    public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
    {
      var aggregateEvent = domainEvent.GetAggregateEvent();
      switch (aggregateEvent)
      {
        case BankStatementExtracted1Event pe:
        {
            foreach (var req  in pe.PredictionRequests)
            {
                yield return BankStatementLineItemId.With(req.Id).ToString();
            }

            break;
        }

        case BankStatementPredicted2Event pe:
        {
            foreach (var req  in pe.PredictionResults)
            {
                yield return BankStatementLineItemId.With(req.Request.Id).ToString();
            }

            break;
        }

        case BankStatementTextMatched3Event pe:
        {
            foreach (var req  in pe.Matched)
            {
                yield return BankStatementLineItemId.With(req.Key.Request.Id).ToString();
            }

            foreach (var req  in pe.Unmatched)
            {
                yield return BankStatementLineItemId.With(req.Request.Id).ToString();
            }

            break;
        }
      }
    }
  }
}