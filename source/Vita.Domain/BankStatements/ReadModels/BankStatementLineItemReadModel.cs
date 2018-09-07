using EventFlow.Entities;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.ReadModels
{
    public class BankStatementLineItemReadModel : Entity<BankStatementLineItemId>
    {
        public string RequestId { get; set; }
        public CategoryType Category { get; set; }
        public string SubCategory { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; }


        public BankStatementLineItemReadModel(BankStatementLineItemId id,
            CategoryType category, string subCategory, string description, decimal amount) : base(id)
        {
            RequestId = id.Value;
            Category = category;
            SubCategory = subCategory;
            Description = description;
            Amount = amount;
        }
    }
}