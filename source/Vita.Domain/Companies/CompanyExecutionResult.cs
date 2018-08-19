using EventFlow.Aggregates.ExecutionResults;

namespace Vita.Domain.Companies
{
    public class CompanyExecutionResult : IExecutionResult
    {
        public bool IsSuccess { get; set; }
    }
}
