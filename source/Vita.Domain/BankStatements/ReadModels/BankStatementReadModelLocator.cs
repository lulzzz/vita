using System.Collections.Generic;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements.ReadModels
{
    /// <summary>
    ///     https://github.com/eventflow/EventFlow/issues/502
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
                    foreach (var id in pe.PredictionRequests.Select(x => x.Id).Distinct())
                        yield return BankStatementLineItemId.With(id).ToString();

                    break;
                }

                case BankStatementPredicted2Event pe:
                {
                    foreach (var req in pe.PredictionResults.Select(x => x.Request.Id).Distinct())
                        yield return BankStatementLineItemId.With(req).ToString();

                    break;
                }

                case BankStatementTextMatched3Event pe:
                {
                    foreach (var req in pe.Matched.Select(x => x.Key.Request.Id).Distinct())
                        yield return BankStatementLineItemId.With(req).ToString();

                    foreach (var req in pe.Unmatched.Select(x => x.Request.Id).Distinct())
                        yield return BankStatementLineItemId.With(req).ToString();

                    break;
                }
            }
        }
    }
}