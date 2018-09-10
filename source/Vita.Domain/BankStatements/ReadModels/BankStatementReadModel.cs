using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using EventFlow.Sql.ReadModels.Attributes;
using Vita.Contracts;
using Vita.Domain.BankStatements.Events;
using Vita.Domain.Infrastructure;
using Vita.Domain.Infrastructure.EventFlow;

//#pragma warning disable 618

namespace Vita.Domain.BankStatements.ReadModels
{
  [Table("BankStatementReadModel")]
  public class BankStatementReadModel : ReadModelBase, IReadModel,
    IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event>,
    IAmReadModelFor<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event>
  {
    [SqlReadModelIdentityColumn] public string RequestId { get; set; }

    public CategoryType Category { get; set; }
    public string SubCategory { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public PredictionMethod Method { get; set; }
    public DateTime? TransactionUtcDate { get; set; }

    //public IEnumerable<PredictionResult> PredictionResults { get; set; }
    //public IEnumerable<PredictionResult> Unmatched { get; set; }
    //public Dictionary<Guid,TextClassificationResult> Matched { get; set; }

    public void Apply(IReadModelContext context,
      IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent)
    {
      AggregateId = domainEvent.AggregateIdentity.Value;

      var rm = domainEvent.AggregateEvent.PredictionResults.SingleOrDefault(x =>
        x.Request.Id.ToString() == context.ReadModelId);

      if (rm != null)
      {
        var id = BankStatementLineItemId.With(Guid.Parse(context.ReadModelId));
        // var model = new BankStatementLineItemReadModel(id, CategoryType.BankingFinance, rm.PredictedValue,rm.Request.Description);

        RequestId = id.Value;
        Category = CategoryTypeConverter.FromSubcategory(rm.PredictedValue);
        SubCategory = rm.PredictedValue;
        Description = rm.Request.Description;
        Amount = Convert.ToDecimal(rm.Request.Amount);
        Method = rm.Method;
        TransactionUtcDate = rm.Request.TransactionUtcDate;
      }
    }

    public void Apply(IReadModelContext context,
      IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent)
    {
      AggregateId = domainEvent.AggregateIdentity.Value;
      var rm = domainEvent.AggregateEvent.Matched.SingleOrDefault(x =>
        x.Item1.Request.Id.ToString() == context.ReadModelId);

      if (rm != null)
      {
        var id = BankStatementLineItemId.With(Guid.Parse(context.ReadModelId));
        // var model = new BankStatementLineItemReadModel(id, CategoryType.BankingFinance, rm.PredictedValue,rm.Request.Description);

        RequestId = id.Value;
        Category = CategoryType.BankingFinance;
        SubCategory = rm.Item2.Classifier.SubCategory;
        Description = rm.Item1.Request.Description;
        Amount = Convert.ToDecimal(rm.Item1.Request.Amount);
        Method = PredictionMethod.KeywordMatch;
        TransactionUtcDate = rm.Item1.Request.TransactionUtcDate;
      }
    }
  }
}