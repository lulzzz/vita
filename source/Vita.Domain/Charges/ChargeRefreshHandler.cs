using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.Search.Text.Response;
using MassTransit;
using Serilog;
using Serilog.Context;
using Vita.Contracts;
using Vita.Domain.Infrastructure;
using Vita.Domain.Places;
using Vita.Domain.Services;


namespace Vita.Domain.Charges
{
  public class ChargeRefreshHandler : CollectionBase, IConsumer<ChargeRefreshRequest>
  {
    private readonly ITextClassifier _textClassifier;
    private readonly IGooglePlacesSearcher _googlePlacesSearcher;
    private readonly IRepository<GoogleApiSearchResponse> _repository;

    public ChargeRefreshHandler(ITextClassifier textClassifier, IGooglePlacesSearcher googlePlacesSearcher, IRepository<GoogleApiSearchResponse> repository)
    {
      _textClassifier = textClassifier;
      _googlePlacesSearcher = googlePlacesSearcher;
      _repository = repository;
    }

    public async Task Consume(ConsumeContext<ChargeRefreshRequest> context)
    {

      using (LogContext.PushProperty("RequestId", context.RequestId))
      using (LogContext.PushProperty("CorrelationId", context.CorrelationId))
      {
        LogVerbose($"received: {context.SentTime}");

        try
        {
          var config = new CosmosDbConfig();
          var repo = new DocumentDbRepository<Charge>(config, Constant.CosmosDbCollections.Charges);

          var charge = await repo.GetItemAsync(context.Message.Id.ToString());
          var place = await GetPlace(charge);
          if (place.Status.HasValue)
          {
            if (place.Status == Status.Ok)
            {
              charge.PlaceLocationTypes = GetPlaceTypes(place.Results).ToList();
              // charge.PlaceId = place.Results.ToList();
              LogDebug($"update try {charge.Id}");
              await repo.UpdateItemAsync(context.Message.Id.ToString(), charge);
              LogInfo($"update ok {charge.Id}");
            }
          }
        }
        catch (Exception e)
        {
          Console.WriteLine(e);
          Log.Error(e, "error {err}", context.Message.Id);
         // throw;
        }
      }

      await Task.Delay(TimeSpan.FromSeconds(3));
    }

    private async Task<PlacesTextSearchResponse> GetPlace(Charge charge)
    {

      // local search first
      try
      {
        var found = _repository.GetAll()
          .Where(x => x.Request != null &&
                      String.Equals(x.Request.SearchTerm,charge.SearchPhrase,
                        StringComparison.InvariantCultureIgnoreCase));

        var responses = found as GoogleApiSearchResponse[] ?? found.ToArray();
        if (responses.Any())
        {
          var first = responses.First();
          return await Task.FromResult(first.PlacesTextSearchResponse);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        LogWarn(e,$"local search error {charge.SearchPhrase}");
      }
      
      var googleSearch = await _googlePlacesSearcher.SearchAsync(charge.SearchPhrase);
      if (googleSearch.Status == Status.Ok)
      {
        LogDebug($"google place search found {charge.SearchPhrase}");
        var item = GoogleApiSearchResponse.Create(charge.SearchPhrase, googleSearch);
        var id = _repository.Insert(item);
        LogInfo($"google place search insert ok  {charge.SearchPhrase} {id}");
      }

      return googleSearch;
    }

    private IEnumerable<PlaceLocationType> GetPlaceTypes(IEnumerable<TextResult> placeResults)
    {
      var list = new List<PlaceLocationType>();
      foreach (var res in placeResults)
      {
        list.AddRange(res.Types.Select(ty => ty.GetValueOrDefault()));
      }

      return list.Distinct();
    }
  }
}