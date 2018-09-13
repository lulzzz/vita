using System.Threading;
using System.Threading.Tasks;

namespace Vita.Api
{
    public interface ISerializedQueryProcessor
    {
        Task<dynamic> ProcessSerilizedQueryAsync(string name, string json, CancellationToken cancellationToken);
    }
}
