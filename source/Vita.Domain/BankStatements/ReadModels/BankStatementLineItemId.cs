using EventFlow.Core;

namespace Vita.Domain.BankStatements.ReadModels
{
    public class BankStatementLineItemId : Identity<BankStatementLineItemId>
    {
        public BankStatementLineItemId(string value) : base(value)
        {
        }
    }
}