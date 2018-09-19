using System;
using EventFlow.Queries;
using Vita.Contracts;

namespace Vita.Domain.BankStatements.Queries
{
    public class BankStatementAnalysisSummaryQuery : IQuery<BankStatementAnalysisSummaryView>
    {
        public BankStatementId BankStatementId { get; set; }
        // default to last 30 days
        public DateTime FromUtcDateTime { get; set; } = DateTime.UtcNow;
        public DateTime? ToUtcDateTime { get; set; }  = DateTime.UtcNow.AddDays(-30);
    }
}
