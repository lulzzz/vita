using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.MsSql.ReadStores.Attributes;
using EventFlow.ReadStores;
using ExtensionMinder;
using Vita.Contracts;
using Vita.Domain.BankStatements.Events;
using Vita.Domain.Infrastructure;
using Vita.Domain.Infrastructure.EventFlow;

//#pragma warning disable 618

namespace Vita.Domain.BankStatements.ReadModels
{
    [Table("BankStatementReadModel")]
    public class BankStatementReadModel : ReadModelBase, IReadModel,
        //IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event>,
        IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event>,
        IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event>
    {
        [MsSqlReadModelIdentityColumn] public string RequestId { get; set; }

        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public PredictionMethod Method { get; set; }
        public DateTime? TransactionUtcDate { get; set; }

        //public IEnumerable<PredictionResult> PredictionResults { get; set; }
        //public IEnumerable<PredictionResult> Unmatched { get; set; }
        //public Dictionary<Guid,TextClassificationResult> Matched { get; set; }

        //public void Apply(IReadModelContext context,
        //  IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementExtracted1Event> domainEvent)
        //{
        //  AggregateId = domainEvent.AggregateIdentity.Value;

        //  var rm = domainEvent.AggregateEvent.PredictionRequests.Single(x =>
        //    x.Id.ToString() == context.ReadModelId.ToVitaGuid().ToString());

        //  var id = context.ReadModelId;
        //  RequestId = id;
        //  Category = CategoryType.Uncategorised.GetDescription();
        //  SubCategory = SubCategories.Uncategorised;
        //  Description = rm.Description;
        //  Amount = Convert.ToDecimal(rm.Amount);
        //  Method = PredictionMethod.MultiClassClassifier;
        //  TransactionUtcDate = rm.TransactionUtcDate;
        //}

        public void Apply(IReadModelContext context,
            IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;

            if (context.ReadModelId == AggregateId.ToString()) throw new ApplicationException();

            var rm = domainEvent.AggregateEvent.PredictionResults.Single(x =>
                x.Request.Id.ToString() == context.ReadModelId.ToVitaGuid().ToString());
            if (rm == null)
            {
                Logger.Warning($"read model not found {context.ReadModelId}");
                return;
            }

            var id = context.ReadModelId;
            var subcategory = GetSubCategory(rm.PredictedValue);

            RequestId = id;
            Category = CategoryTypeConverter.FromSubcategory(subcategory).GetDescription();
            SubCategory = GetSubCategory(subcategory);
            Description = rm.Request.Description;
            Amount = Convert.ToDecimal(rm.Request.Amount);
            Method = PredictionMethod.MultiClassClassifier;
            TransactionUtcDate = rm.Request.TransactionUtcDate;
        }

        public void Apply(IReadModelContext context,
            IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent)
        {
            AggregateId = domainEvent.AggregateIdentity.Value;
            if (context.ReadModelId == AggregateId.ToString()) throw new ApplicationException();

            var rm = domainEvent.AggregateEvent.Matched.Single(x =>
                x.Key.Request.Id.ToString() == context.ReadModelId.ToVitaGuid().ToString());
            if (rm.Key == null)
            {
                Logger.Warning($"read model not found {context.ReadModelId}");
                return;
            }

            var subcategory = GetSubCategory(rm.Value.Classifier?.SubCategory);

            if (subcategory == SubCategories.Uncategorised)
                Logger.Warning("SubCategory == SubCategories.Uncategorised", rm.Key.Request.Description);

            var id = context.ReadModelId;
            if (string.IsNullOrEmpty(id))
            {
                Logger.Warning($"read model no id found {context.ReadModelId}");
                return;
            }

            RequestId = id;
            Category = CategoryTypeConverter.FromSubcategory(subcategory).GetDescription();
            SubCategory = subcategory;
            Description = rm.Key.Request.Description;
            Amount = Convert.ToDecimal(rm.Key.Request.Amount);
            Method = PredictionMethod.KeywordMatch;
            TransactionUtcDate = rm.Key.Request.TransactionUtcDate;
        }


        private string GetSubCategory(string text)
        {
            if (CategoryTypeConverter.IsValidSubCategory(text)) return text;

            if (text == SubCategories.Uncategorised) return text;

            if (text.ToLower() == "wages") return SubCategories.Income.SalaryWages;

            throw new ApplicationException($"not valid subcategory: {text}");
        }
    }
}