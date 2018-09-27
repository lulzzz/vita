using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Core;
using EventFlow.Logs;
using EventFlow.Queries;
using Vita.Domain.Infrastructure.EventFlow;

namespace Vita.Domain.Services
{
    public class SerializedQueryProcessor : ISerializedQueryProcessor
    {
        private readonly ILog _log;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IQueryProcessor _queryProcessor;
        public static QueryDefinition[] QueryDefinitions => BuildQueryDefinitions();
        public static readonly Assembly DomainAssembly = typeof(SerializedQueryProcessor).Assembly;

        public SerializedQueryProcessor(ILog log, IJsonSerializer jsonSerializer, IQueryProcessor queryProcessor)
        {
            _log = log;
            _jsonSerializer = jsonSerializer;
            _queryProcessor = queryProcessor;
        }

        public async Task<dynamic> ProcessSerilizedQueryAsync(string name, string json, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));

            _log.Verbose($"Executing serilized query '{name}'");

            var queryDef = QueryDefinitions.SingleOrDefault(t => t.QueryType.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
            if (queryDef == null)
            {
                throw new ArgumentException($"No query definition found for query '{name}'");
            }

            dynamic query;
            try
            {
                query = _jsonSerializer.Deserialize(json, queryDef.QueryType);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Failed to deserilize query '{name}': {e.Message}", e);
            }

            return await _queryProcessor.ProcessAsync(query, cancellationToken).ConfigureAwait(false);
        }

        private static QueryDefinition[] BuildQueryDefinitions()
        {
            bool IsIQueryInterface(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQuery<>);
            var defs = DomainAssembly.GetTypes()
                .Where(t => t.GetInterfaces().Any(IsIQueryInterface))
                .Select(t => new QueryDefinition
                {
                    Name = t.Name,
                    QueryType = t,
                    ResultType = t.GetInterfaces().Single(IsIQueryInterface).GetGenericArguments().Single()
                })
                .ToArray();

            return defs;
        }

    }
}