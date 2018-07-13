using System;
using System.Threading.Tasks;
using GoogleApi.Entities.Common;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.Search.Text.Request;
using GoogleApi.Entities.Places.Search.Text.Response;
using Serilog;
using Vita.Contracts;

namespace Vita.Domain.Services
{
  public class GooglePlacesSearcher : IGooglePlacesSearcher
  {
    public async Task<PlacesTextSearchResponse> SearchAsync(string phrase)
    {
      Log.Information($"GooglePlacesSearcher : {phrase}");
      var request = new PlacesTextSearchRequest
      {
        Key = Constant.ApiKey.GoogleApiLendZen,
        Query = phrase,
        Location = new Location("Australia"),
        Radius = 50
      };

      try
      {
        var response = await GoogleApi.GooglePlaces.TextSearch.QueryAsync(request);

        switch (response.Status)
        {
          case Status.Ok:
          case Status.ZeroResults:
            Log.Information("{status} {phrase}", response.Status.GetValueOrDefault(), request.Query);
            return response;
          case Status.OverQueryLimit:
          case Status.MaxElementsExceeded:
            Log.Warning("{status} {searchterm} {response}",response.Status.GetValueOrDefault(), request.Query, response.ErrorMessage);
            return response;
          default:
            Log.Error("{status} {searchterm} {error}", response.Status.GetValueOrDefault(), request.Query, response.ErrorMessage);
            return response;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        Log.Error(e, "GooglePlacesSearcher error {text}", phrase);
        throw;
      }
    }
  }
}