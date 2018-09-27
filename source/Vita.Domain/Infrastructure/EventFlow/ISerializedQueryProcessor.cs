using System.Threading;
using System.Threading.Tasks;

namespace Vita.Domain.Infrastructure.EventFlow
{
    public interface ISerializedQueryProcessor
    {
        Task<dynamic> ProcessSerilizedQueryAsync(string name, string json, CancellationToken cancellationToken);
    }
}
