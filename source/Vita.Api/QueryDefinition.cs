using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vita.Api
{
    public class QueryDefinition
    {
        public string Name { get; set; }
        public Type QueryType { get; set; }
        public Type ResultType { get; set; }
    }
}
