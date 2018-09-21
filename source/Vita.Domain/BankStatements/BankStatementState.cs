using System;
using System.Collections.Generic;
using EventFlow.Aggregates;
using Vita.Contracts;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements
{
    public class BankStatementState : AggregateState<BankStatementAggregate, BankStatementId, BankStatementState>,
        IEmit<BankStatementExtracted1Event>,
        IEmit<BankStatementPredicted2Event>,
        IEmit<BankStatementTextMatched3Event>
    {
        public IEnumerable<PredictionRequest> PredictionRequests { get; set; }
        public IEnumerable<PredictionResult> PredictionResults { get; set; }
        public IList<KeyValuePair<PredictionResult, TextClassificationResult>> Matched { get; set; }

        public void Apply(BankStatementExtracted1Event aggregateEvent)
        {
            PredictionRequests = aggregateEvent.PredictionRequests;
        }

        public void Apply(BankStatementPredicted2Event aggregateEvent)
        {
            PredictionResults = aggregateEvent.PredictionResults;
        }

        public void Apply(BankStatementTextMatched3Event aggregateEvent)
        {
            Matched = aggregateEvent.Matched;
        }
    }
}