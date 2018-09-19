using System;
using EventFlow.Queries;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Queries
{
    public class BankStatementAnalysisSummaryQuery : IQuery<BankStatementAnalysisSummaryView>
    {
        public BankStatementId BankStatementId { get; set; }
        // default one year
        public DateTime FromUtcDateTime { get; set; } = DateTime.UtcNow.AddYears(-1);
        public DateTime ToUtcDateTime { get; set; }  = DateTime.UtcNow;
    }
}
