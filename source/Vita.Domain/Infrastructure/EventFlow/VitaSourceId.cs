using System;
using System.Collections.Generic;
using System.Text;
using EventFlow.Core;
using Newtonsoft.Json;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public class VitaSourceId : SourceId
    {
        [JsonConstructor]
        public VitaSourceId(string value) : base(value)
        {
        }
    }
}
