using System;
using System.ComponentModel.DataAnnotations.Schema;
using EventFlow.Aggregates;
using EventFlow.Entities;
using EventFlow.MsSql.ReadStores.Attributes;
using EventFlow.ReadStores;
using Vita.Contracts;
using Vita.Domain.BankStatements.Events;
using Vita.Domain.Infrastructure.EventFlow;

namespace Vita.Domain.BankStatements.ReadModels
{
    [Table("BankStatementReadModel")]
    public class BankStatementLineItemReadModel  : ReadModelBase, IReadModel,
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


        public void Apply(IReadModelContext context, IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementPredicted2Event> domainEvent)
        {
             
        }

        public void Apply(IReadModelContext context, IDomainEvent<BankStatementAggregate, BankStatementId, BankStatementTextMatched3Event> domainEvent)
        {
             
        }
    }
}