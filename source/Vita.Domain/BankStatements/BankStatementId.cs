using EventFlow.Core;

namespace Vita.Domain.BankStatements
{
    public class BankStatementId: Identity<BankStatementId>
    {
        public BankStatementId(string value) : base(value)
        {
        }
    }
}
