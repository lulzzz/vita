using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.Azure.Search;

namespace Vita.Contracts
{
  public class Locality : Tracking
  {
    [Key] public Guid Id { get; set; }
    [IsSearchable] public string Postcode { get; set; }
    [IsSearchable] public string Suburb { get; set; }

    [IsFilterable]
    [IsSortable]
    [IsFacetable]
    public AustralianState? AustralianState { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string PlaceId { get; set; }

    public override string ToString()
    {
      return $"{nameof(Id)}: {Id}, {nameof(Postcode)}: {Postcode}, {nameof(Suburb)}: {Suburb}, {nameof(AustralianState)}: {AustralianState}";
    }
  }
}