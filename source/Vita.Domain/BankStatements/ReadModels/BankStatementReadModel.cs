using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Newtonsoft.Json;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
using Vita.Domain.BankStatements.Events;
using Vita.Domain.Infrastructure.EventFlow;

//#pragma warning disable 618

namespace Vita.Domain.BankStatements.ReadModels
{
    [Table("BankStatementReadModel")]
    public class BankStatementReadModel  : ReadModelBase,IReadModel,
        IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event>,
        IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event>
    {
        [SqlReadModelIdentityColumn]
        public string RequestId { get; set; }
        public CategoryType Category { get; set; }
        public string SubCategory { get; set; }
        public string Description { get; set; }


        //public IEnumerable<PredictionResult> PredictionResults { get; set; }
        //public IEnumerable<PredictionResult> Unmatched { get; set; }
        //public Dictionary<Guid,TextClassificationResult> Matched { get; set; }

        public void Apply(IReadModelContext context, IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;

            var rm = domainEvent.AggregateEvent.PredictionResults.SingleOrDefault(x =>
                x.Request.Id.ToString() == context.ReadModelId);

            if (rm != null)
            {
                var id = BankStatementLineItemId.With(Guid.Parse(context.ReadModelId));
               // var model = new BankStatementLineItemReadModel(id, CategoryType.BankingFinance, rm.PredictedValue,rm.Request.Description);

                RequestId = id.Value;
                Category = CategoryType.BankingFinance;
                SubCategory = rm.PredictedValue;
                Description = rm.Request.Description;
            }
        }

        public void Apply(IReadModelContext context, IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent)
        {            
            AggregateId = domainEvent.AggregateIdentity.Value;
            //RequestId = "missing-" + Guid.NewGuid().ToString();
            //SubCategory = Categories.Uncategorised;
            //Category = CategoryType.Uncategorised;
            //if (Matched == null) Matched = new Dictionary<Guid, TextClassificationResult>();

            //Unmatched = domainEvent.AggregateEvent.Unmatched;

            //foreach (var predictionResult in  domainEvent.AggregateEvent.Matched)
            //{
            //    Matched.Add(predictionResult.Item1.Request.Id, predictionResult.Item2);   
            //}
        }
    }
}
