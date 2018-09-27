using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Commands;
using EventFlow.Core;
using EventFlow.Logs;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public class CustomSerializedCommandPublisher : ICustomSerializedCommandPublisher
    {
        public static CommandDefinition[] Definitions => BuildDefinitions();
        public static readonly Assembly DomainAssembly = typeof(CustomSerializedCommandPublisher).Assembly;
        private readonly ILog _log;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly ICommandBus _commandBus;

        public CustomSerializedCommandPublisher(ILog log, IJsonSerializer jsonSerializer,ICommandBus commandBus)
        {
            _log = log;
            _jsonSerializer = jsonSerializer;
            _commandBus = commandBus;
        }

        private static CommandDefinition[] BuildDefinitions()
        {
            //bool IsCommand(Type i) => i.IsGenericType && i.GetGenericTypeDefinition() == typeof( Command<,,>);

            var asses = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(x => x.GetName().Name.StartsWith("Vita"));

            var defs = asses
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(ICommand).IsAssignableFrom(x))
                .Select(t => new CommandDefinition
                {
                    Name = t.Name,
                    CommandType = t,
                })
                .ToList();

            return defs.ToArray();
        }

        public async Task<ISourceId> PublishSerilizedCommandAsync(string name, int version, string json, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(json)) throw new ArgumentNullException(nameof(json));

            _log.Verbose($"Executing serilized command '{name}'");

            var commandDefinition = Definitions.SingleOrDefault(x => x.Name == name);
            if (commandDefinition==null) throw new ArgumentException($"Failed to find command DEFINITION '{name}' v{version}: {json}");

            ICommand command;
            try
            {
                command = (ICommand)_jsonSerializer.Deserialize(json, commandDefinition.CommandType);
            }
            catch (Exception e)
            {
                throw new ArgumentException($"Failed to deserialize command '{name}' v{version}: {e.Message}", e);
            }

            await command.PublishAsync(_commandBus, cancellationToken).ConfigureAwait(false);
            return command.GetSourceId();
        }
    }
}
