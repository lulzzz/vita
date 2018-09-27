using System;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public class QueryDefinition
    {
        public string Name { get; set; }
        public Type QueryType { get; set; }
        public Type ResultType { get; set; }
    }
}
