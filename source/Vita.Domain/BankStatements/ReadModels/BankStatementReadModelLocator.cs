using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements.ReadModels
{
    /// <summary>
    /// https://github.com/eventflow/EventFlow/issues/502
    /// </summary>
    public class BankStatementReadModelLocator  : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
     
            var aggregateEvent = domainEvent.GetAggregateEvent();
            switch (aggregateEvent)
            {
                case BankStatementPredicted2Event predicted2Event:
                    foreach (var message in predicted2Event.PredictionResults.Where(x=>!string.IsNullOrEmpty(x.Request.Description)))
                    {
                        yield return message.Request.Id.ToString();
                    }
                    break;

                case BankStatementTextMatched3Event textMatched3Event:
                    foreach (var message in textMatched3Event.ExactMatched)
                    {
                        yield return message.Item1.Request.Id.ToString();
                    }
                    break;
            }
        }
    }
}
