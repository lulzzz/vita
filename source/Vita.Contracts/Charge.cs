using System;
using System.Collections.Generic;
using GoogleApi.Entities.Common.Enums;
using Microsoft.Azure.Search;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vita.Contracts
{
  public class Charge : Tracking
  {
    [System.ComponentModel.DataAnnotations.Key]
    public Guid Id { get; set; }

    [IsSearchable]
    public string SearchPhrase { get; set; }
    [IsSearchable]
    public string Keywords { get; set; }
    [IsSearchable]
    public string Notes { get; set; }
    [IsSearchable]
    public string Tags { get; set; }
    [IsFilterable, IsSortable]
    public DateTime? TransactionUtcDate { get; set; }
    [IsFilterable, IsSortable]
    public DateTimeOffset? PostedDate { get; set; }

    // who -- company who billed eg McDonalds
    [IsSearchable]
    public Guid? CompanyId { get; set; }
    [IsSearchable, IsFilterable, IsSortable]
    public string BankCode { get; set; }
    [IsSearchable, IsFilterable, IsSortable]
    public string BankName { get; set; }
    [IsSearchable]
    public string Bsb { get; set; }
    [IsSearchable]
    public string AccountNumber { get; set; }
    [IsSearchable]
    public string AccountName { get; set; }

    public double? Amount { get; set; }
    public double? Balance { get; set; }
    public double? BalanceAvailable { get; set; }

    // what -- was this transaction debit/credit/reversal/fees/interest
    [JsonConverter(typeof(StringEnumConverter))]
    [IsFilterable, IsSortable]
    public TransactionType? TransactionType { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    [IsFilterable, IsSortable]
    public ExpenseType ExpenseType { get; set; }

    // where -- did the exchange occur
    [IsSearchable]
    public Guid? LocalityId { get; set; }

    [IsSearchable]
    public virtual string PlaceId { get; set; }

    // why -- Category
    [JsonConverter(typeof(StringEnumConverter))]
    [IsSearchable, IsFilterable, IsSortable]
    public CategoryType Category { get; set; }

    [IsSearchable, IsFilterable]
    public string SubCategory { get; set; }

    [IsSearchable, IsFilterable, IsSortable]
    public IList<PlaceLocationType> PlaceLocationTypes { get; set; }

    // how -- eftpos, credit card, direct debit etc
    [JsonConverter(typeof(StringEnumConverter))]
    [IsSearchable, IsFilterable, IsSortable]
    public PaymentMethodType PaymentMethod { get; set; }


    // type and string
    public Dictionary<string,string> JsonData { get; set; }

    public string ToChargeId()
    {
      return $"charge-{this.Id.ToString()}";
    }
  }
}