using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Core;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public interface ICustomSerializedCommandPublisher
    {
        Task<ISourceId> PublishSerilizedCommandAsync(string name, int version, string json, CancellationToken cancellationToken);
    }
}
