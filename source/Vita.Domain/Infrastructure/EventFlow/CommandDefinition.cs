using System;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public class CommandDefinition
    {
        public string Name { get; set; }
        public Type CommandType { get; set; }
    }
}