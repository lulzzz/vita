using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using Vita.Contracts;
using Vita.Domain.BankStatements.Events;

namespace Vita.Domain.BankStatements.ReadModels
{
    [Table("BankStatementReadModel")]
    public class BankStatementReadModel  : IReadModel,
        IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event>,
        IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event>
    {

        public IEnumerable<PredictionResult> PredictionResults { get; set; }
        public IEnumerable<PredictionResult> Unmatched { get; set; }
        public Dictionary<Guid,TextClassificationResult> Matched { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent)
        {
            this.PredictionResults = domainEvent.AggregateEvent.PredictionResults;

        }

        public void Apply(IReadModelContext context, IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent)
        {            
            if (Matched == null) Matched = new Dictionary<Guid, TextClassificationResult>();

            Unmatched = domainEvent.AggregateEvent.Unmatched;

            foreach (var predictionResult in  domainEvent.AggregateEvent.Matched)
            {
                Matched.Add(predictionResult.Item1.Request.Id, predictionResult.Item2);   
            }
        }
    }
}
