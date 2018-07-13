using System.Threading.Tasks;
using GoogleApi.Entities.Places.Search.Text.Response;

namespace Vita.Domain.Services
{
    public interface IGooglePlacesSearcher
    {
      Task<PlacesTextSearchResponse> SearchAsync(string phrase);
    }
}
