using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vita.Contracts.ChargeId
{
  public class SearchResponse
  {
    [JsonProperty(PropertyName = "Id")] public Guid Id { get; set; }

    public Company Who { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public TransactionType? What { get; set; }

    public Locality Where { get; set; }

    public DateTime? When { get; set; } // utc

    //TODO change this or will leak keywords
    //public IDictionary<CategoryType, string> Why { get; set; }
    public Classifier Why { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public PaymentMethodType? PaymentMethodType { get; set; }

    public DateTime CreatedUtcDate { get; set; }

    public bool IsChargeId { get; set; } = false;

    public Guid? ChargeId { get; set; }
  }
}