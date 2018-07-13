using System;

namespace Vita.Domain.Places
{
    public class GoogleApiSearchResponse
  {
      public Guid Id { get; set; } = Guid.NewGuid();
      public GoogleApiSearchRequest Request { get; set; }
      public GoogleApi.Entities.Places.Search.Text.Response.PlacesTextSearchResponse PlacesTextSearchResponse { get; set; }

    public static GoogleApiSearchResponse Create(string searchPhrase, GoogleApi.Entities.Places.Search.Text.Response.PlacesTextSearchResponse response)
    {
      var item = new GoogleApiSearchResponse
      {
        Id = Guid.NewGuid(),
        PlacesTextSearchResponse = response,
        Request = new GoogleApiSearchRequest() { SearchTerm = searchPhrase }
      };

      return item;
    }
  }
}
