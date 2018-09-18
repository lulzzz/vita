using System.Threading;
using System.Threading.Tasks;

namespace Vita.Domain.Services
{
    public interface ISerializedQueryProcessor
    {
        Task<dynamic> ProcessSerilizedQueryAsync(string name, string json, CancellationToken cancellationToken);
    }
}
