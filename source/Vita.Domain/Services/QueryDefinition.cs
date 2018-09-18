using System;

namespace Vita.Domain.Services
{
    public class QueryDefinition
    {
        public string Name { get; set; }
        public Type QueryType { get; set; }
        public Type ResultType { get; set; }
    }
}
