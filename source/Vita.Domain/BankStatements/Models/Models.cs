// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using Vita.Domain.BankStatements.Models;
//
//    var fetchAllResponse = FetchAllResponse.FromJson(jsonString);

namespace Vita.Domain.BankStatements.Models
{
  using System;
  using System.Collections.Generic;

  using System.Globalization;
  using Newtonsoft.Json;
  using Newtonsoft.Json.Converters;

  public partial class FetchAllResponse
  {
    [JsonProperty("accounts")]
    public List<Account> Accounts { get; set; }

    [JsonProperty("user_token")]
    public string UserToken { get; set; }

    [JsonProperty("referral_code")]
    public string ReferralCode { get; set; }
  }

  public partial class Account
  {
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("accountNumber")]
    public string AccountNumber { get; set; }

    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("bsb")]
    public string Bsb { get; set; }

    [JsonProperty("balance")]
    public double Balance { get; set; }

    [JsonProperty("available")]
    public double Available { get; set; }

    [JsonProperty("accountType")]
    public string AccountType { get; set; }

    [JsonProperty("accountHolder")]
    public string AccountHolder { get; set; }

    [JsonProperty("statementData")]
    public StatementData StatementData { get; set; }

    [JsonProperty("institution")]
    public string Institution { get; set; }
  }

  public partial class StatementData
  {
    [JsonProperty("details")]
    public List<Detail> Details { get; set; }

    [JsonProperty("totalCredits")]
    public string TotalCredits { get; set; }

    [JsonProperty("totalDebits")]
    public string TotalDebits { get; set; }

    [JsonProperty("openingBalance")]
    public string OpeningBalance { get; set; }

    [JsonProperty("closingBalance")]
    public string ClosingBalance { get; set; }

    [JsonProperty("startDate")]
    public string StartDate { get; set; }

    [JsonProperty("endDate")]
    public string EndDate { get; set; }

    [JsonProperty("minBalance")]
    public string MinBalance { get; set; }

    [JsonProperty("maxBalance")]
    public string MaxBalance { get; set; }

    [JsonProperty("dayEndBalances")]
    public List<DayEndBalance> DayEndBalances { get; set; }

    [JsonProperty("minDayEndBalance")]
    public string MinDayEndBalance { get; set; }

    [JsonProperty("maxDayEndBalance")]
    public string MaxDayEndBalance { get; set; }

    [JsonProperty("daysInNegative")]
    public long DaysInNegative { get; set; }

    [JsonProperty("errorMessage")]
    public string ErrorMessage { get; set; }

    [JsonProperty("analysis")]
    public Analysis Analysis { get; set; }
  }

  public partial class Analysis
  {
    [JsonProperty("Income")]
    public Income Income { get; set; }

    [JsonProperty("Benefits")]
    public Benefits Benefits { get; set; }

    [JsonProperty("Dishonours")]
    public Dishonours Dishonours { get; set; }

    [JsonProperty("Loans")]
    public Loans Loans { get; set; }

    [JsonProperty("Rent")]
    public Rent Rent { get; set; }

    [JsonProperty("Gambling")]
    public Gambling Gambling { get; set; }

    [JsonProperty("Other Debits")]
    public OtherDebits OtherDebits { get; set; }
  }

  public partial class Benefits
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("Maternity Payment")]
    public LospusicCougar MaternityPayment { get; set; }

    [JsonProperty("Family Benefits")]
    public LospusicCougar FamilyBenefits { get; set; }

    [JsonProperty("Schoolkids Bonus")]
    public LospusicCougar SchoolkidsBonus { get; set; }

    [JsonProperty("Parenting Payment")]
    public LospusicCougar ParentingPayment { get; set; }

    [JsonProperty("Carers Benefits")]
    public LospusicCougar CarersBenefits { get; set; }

    [JsonProperty("Youth Allowance")]
    public LospusicCougar YouthAllowance { get; set; }

    [JsonProperty("Newstart")]
    public LospusicCougar Newstart { get; set; }

    [JsonProperty("Pension")]
    public LospusicCougar Pension { get; set; }

    [JsonProperty("AusStudy")]
    public LospusicCougar AusStudy { get; set; }

    [JsonProperty("Medicare")]
    public LospusicCougar Medicare { get; set; }

    [JsonProperty("Other")]
    public LospusicCougar Other { get; set; }
  }

  public partial class LospusicCougar
  {
    [JsonProperty("transactionCount")]
    public long TransactionCount { get; set; }

    [JsonProperty("totalValue")]
    public long TotalValue { get; set; }

    [JsonProperty("monthAvg")]
    public long MonthAvg { get; set; }

    [JsonProperty("minValue")]
    public double MinValue { get; set; }

    [JsonProperty("maxValue")]
    public double MaxValue { get; set; }

    [JsonProperty("firstTransaction")]
    public object FirstTransaction { get; set; }

    [JsonProperty("lastTransaction")]
    public object LastTransaction { get; set; }

    [JsonProperty("period")]
    public double Period { get; set; }

    [JsonProperty("periodIsRegular")]
    public bool PeriodIsRegular { get; set; }

    [JsonProperty("transactions")]
    public List<object> Transactions { get; set; }
  }

  public partial class Total
  {
    [JsonProperty("transactionCount")]
    public long TransactionCount { get; set; }

    [JsonProperty("totalValue")]
    public long TotalValue { get; set; }

    [JsonProperty("monthAvg")]
    public double MonthAvg { get; set; }
  }

  public partial class Dishonours
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("Overdrawn")]
    public LospusicCougar Overdrawn { get; set; }

    [JsonProperty("Dishonour")]
    public LospusicCougar Dishonour { get; set; }

    [JsonProperty("Return")]
    public LospusicCougar Return { get; set; }

    [JsonProperty("Reversal")]
    public LospusicCougar Reversal { get; set; }
  }

  public partial class Gambling
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("10Bet")]
    public LospusicCougar The10Bet { get; set; }

    [JsonProperty("AusBet")]
    public LospusicCougar AusBet { get; set; }

    [JsonProperty("Bet365")]
    public LospusicCougar Bet365 { get; set; }

    [JsonProperty("BetFair")]
    public LospusicCougar BetFair { get; set; }

    [JsonProperty("BetStar")]
    public LospusicCougar BetStar { get; set; }

    [JsonProperty("Bookmaker")]
    public LospusicCougar Bookmaker { get; set; }

    [JsonProperty("Centrebet")]
    public LospusicCougar Centrebet { get; set; }

    [JsonProperty("ClassicBet")]
    public LospusicCougar ClassicBet { get; set; }

    [JsonProperty("CrownBet")]
    public LospusicCougar CrownBet { get; set; }

    [JsonProperty("DigiMedia")]
    public LospusicCougar DigiMedia { get; set; }

    [JsonProperty("Ladbrokes")]
    public LospusicCougar Ladbrokes { get; set; }

    [JsonProperty("LeoVegas")]
    public LospusicCougar LeoVegas { get; set; }

    [JsonProperty("Luxbet")]
    public LospusicCougar Luxbet { get; set; }

    [JsonProperty("OzBet")]
    public LospusicCougar OzBet { get; set; }

    [JsonProperty("Palmerbet")]
    public LospusicCougar Palmerbet { get; set; }

    [JsonProperty("Punters")]
    public LospusicCougar Punters { get; set; }

    [JsonProperty("Pokerstars")]
    public LospusicCougar Pokerstars { get; set; }

    [JsonProperty("Roxy Palace")]
    public LospusicCougar RoxyPalace { get; set; }

    [JsonProperty("Sportsbet")]
    public LospusicCougar Sportsbet { get; set; }

    [JsonProperty("Sportsbetting")]
    public LospusicCougar Sportsbetting { get; set; }

    [JsonProperty("Sportingbet")]
    public LospusicCougar Sportingbet { get; set; }

    [JsonProperty("TABTouch")]
    public LospusicCougar TabTouch { get; set; }

    [JsonProperty("Tatts")]
    public LospusicCougar Tatts { get; set; }

    [JsonProperty("The Palace Group")]
    public LospusicCougar ThePalaceGroup { get; set; }

    [JsonProperty("Tom Waterhouse")]
    public LospusicCougar TomWaterhouse { get; set; }

    [JsonProperty("UBET")]
    public LospusicCougar Ubet { get; set; }

    [JsonProperty("Unibet")]
    public LospusicCougar Unibet { get; set; }

    [JsonProperty("William Hill")]
    public LospusicCougar WilliamHill { get; set; }

    [JsonProperty("Casino")]
    public LospusicCougar Casino { get; set; }

    [JsonProperty("Lottery")]
    public LospusicCougar Lottery { get; set; }
  }

  public partial class Income
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("Wages")]
    public LospusicCougar Wages { get; set; }

    [JsonProperty("Rent")]
    public LospusicCougar Rent { get; set; }

    [JsonProperty("Interest")]
    public LospusicCougar Interest { get; set; }
  }

  public partial class Loans
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("Amazing Loans")]
    public LospusicCougar AmazingLoans { get; set; }

    [JsonProperty("AMX Money")]
    public LospusicCougar AmxMoney { get; set; }

    [JsonProperty("Aus Financial")]
    public LospusicCougar AusFinancial { get; set; }

    [JsonProperty("Aussie")]
    public LospusicCougar Aussie { get; set; }

    [JsonProperty("Aussie Cash")]
    public LospusicCougar AussieCash { get; set; }

    [JsonProperty("Baycorp")]
    public LospusicCougar Baycorp { get; set; }

    [JsonProperty("Cannon Finance")]
    public LospusicCougar CannonFinance { get; set; }

    [JsonProperty("Cash Advance")]
    public LospusicCougar CashAdvance { get; set; }

    [JsonProperty("Cash Converters")]
    public LospusicCougar CashConverters { get; set; }

    [JsonProperty("Cash Doctors")]
    public LospusicCougar CashDoctors { get; set; }

    [JsonProperty("Cash First")]
    public LospusicCougar CashFirst { get; set; }

    [JsonProperty("Cash in a Flash")]
    public LospusicCougar CashInAFlash { get; set; }

    [JsonProperty("Cash Money")]
    public LospusicCougar CashMoney { get; set; }

    [JsonProperty("Cash Now")]
    public LospusicCougar CashNow { get; set; }

    [JsonProperty("Cash Oz")]
    public LospusicCougar CashOz { get; set; }

    [JsonProperty("Cash Solutions")]
    public LospusicCougar CashSolutions { get; set; }

    [JsonProperty("Cash Smart")]
    public LospusicCougar CashSmart { get; set; }

    [JsonProperty("Cash Stop")]
    public LospusicCougar CashStop { get; set; }

    [JsonProperty("CashToday")]
    public LospusicCougar CashToday { get; set; }

    [JsonProperty("Cash Train")]
    public LospusicCougar CashTrain { get; set; }

    [JsonProperty("Center One")]
    public LospusicCougar CenterOne { get; set; }

    [JsonProperty("Champion Loans")]
    public LospusicCougar ChampionLoans { get; set; }

    [JsonProperty("Champion Money")]
    public LospusicCougar ChampionMoney { get; set; }

    [JsonProperty("City Finance")]
    public LospusicCougar CityFinance { get; set; }

    [JsonProperty("Clear Cash")]
    public LospusicCougar ClearCash { get; set; }

    [JsonProperty("Credit24")]
    public LospusicCougar Credit24 { get; set; }

    [JsonProperty("Credit Corp")]
    public LospusicCougar CreditCorp { get; set; }

    [JsonProperty("Debt Fix")]
    public LospusicCougar DebtFix { get; set; }

    [JsonProperty("Dollars Direct")]
    public LospusicCougar DollarsDirect { get; set; }

    [JsonProperty("Easy Financing")]
    public LospusicCougar EasyFinancing { get; set; }

    [JsonProperty("Fair Go Finance")]
    public FairGoFinance FairGoFinance { get; set; }

    [JsonProperty("Ferratum")]
    public LospusicCougar Ferratum { get; set; }

    [JsonProperty("First Stop Money")]
    public LospusicCougar FirstStopMoney { get; set; }

    [JsonProperty("Fox Symes")]
    public LospusicCougar FoxSymes { get; set; }

    [JsonProperty("FundCo")]
    public LospusicCougar FundCo { get; set; }

    [JsonProperty("Jacaranda Finance")]
    public LospusicCougar JacarandaFinance { get; set; }

    [JsonProperty("Jet Lending")]
    public LospusicCougar JetLending { get; set; }

    [JsonProperty("JP Creditline")]
    public LospusicCougar JpCreditline { get; set; }

    [JsonProperty("K24")]
    public LospusicCougar K24 { get; set; }

    [JsonProperty("Kangaroo Payday")]
    public LospusicCougar KangarooPayday { get; set; }

    [JsonProperty("Kreditech")]
    public LospusicCougar Kreditech { get; set; }

    [JsonProperty("Kwik Finance")]
    public LospusicCougar KwikFinance { get; set; }

    [JsonProperty("Lion Finance")]
    public LospusicCougar LionFinance { get; set; }

    [JsonProperty("Loans by Phone")]
    public LospusicCougar LoansByPhone { get; set; }

    [JsonProperty("Loan Ranger")]
    public LospusicCougar LoanRanger { get; set; }

    [JsonProperty("Max Finance")]
    public LospusicCougar MaxFinance { get; set; }

    [JsonProperty("Max Funding")]
    public LospusicCougar MaxFunding { get; set; }

    [JsonProperty("MoneyCentre")]
    public LospusicCougar MoneyCentre { get; set; }

    [JsonProperty("MoneyMe")]
    public LospusicCougar MoneyMe { get; set; }

    [JsonProperty("Money Now")]
    public LospusicCougar MoneyNow { get; set; }

    [JsonProperty("MoneyPlus")]
    public LospusicCougar MoneyPlus { get; set; }

    [JsonProperty("Money3")]
    public LospusicCougar Money3 { get; set; }

    [JsonProperty("MoneyStart")]
    public LospusicCougar MoneyStart { get; set; }

    [JsonProperty("Morgan Finance")]
    public LospusicCougar MorganFinance { get; set; }

    [JsonProperty("My Budget")]
    public LospusicCougar MyBudget { get; set; }

    [JsonProperty("Needy Money")]
    public LospusicCougar NeedyMoney { get; set; }

    [JsonProperty("Nimble")]
    public LospusicCougar Nimble { get; set; }

    [JsonProperty("OK Money")]
    public LospusicCougar OkMoney { get; set; }

    [JsonProperty("Payday King")]
    public LospusicCougar PaydayKing { get; set; }

    [JsonProperty("Payday Land")]
    public LospusicCougar PaydayLand { get; set; }

    [JsonProperty("Payday Mate")]
    public LospusicCougar PaydayMate { get; set; }

    [JsonProperty("Payday247")]
    public LospusicCougar Payday247 { get; set; }

    [JsonProperty("Perfect Payday")]
    public LospusicCougar PerfectPayday { get; set; }

    [JsonProperty("Personal Finance Co")]
    public LospusicCougar PersonalFinanceCo { get; set; }

    [JsonProperty("Prime Lenders")]
    public LospusicCougar PrimeLenders { get; set; }

    [JsonProperty("Rapid Loans")]
    public LospusicCougar RapidLoans { get; set; }

    [JsonProperty("Secure Funding")]
    public LospusicCougar SecureFunding { get; set; }

    [JsonProperty("Speedy Finance")]
    public LospusicCougar SpeedyFinance { get; set; }

    [JsonProperty("Speedy Money")]
    public LospusicCougar SpeedyMoney { get; set; }

    [JsonProperty("Spot Loans")]
    public LospusicCougar SpotLoans { get; set; }

    [JsonProperty("SRG Finance")]
    public LospusicCougar SrgFinance { get; set; }

    [JsonProperty("Stress Less")]
    public LospusicCougar StressLess { get; set; }

    [JsonProperty("Sunshine Loans")]
    public LospusicCougar SunshineLoans { get; set; }

    [JsonProperty("Swoosh Finance")]
    public LospusicCougar SwooshFinance { get; set; }

    [JsonProperty("Tele Loans")]
    public LospusicCougar TeleLoans { get; set; }

    [JsonProperty("Thorn")]
    public LospusicCougar Thorn { get; set; }

    [JsonProperty("Victory Funding")]
    public LospusicCougar VictoryFunding { get; set; }

    [JsonProperty("Web Moneyline")]
    public LospusicCougar WebMoneyline { get; set; }

    [JsonProperty("Yes Loans")]
    public LospusicCougar YesLoans { get; set; }

    [JsonProperty("Collection House")]
    public LospusicCougar CollectionHouse { get; set; }

    [JsonProperty("Toyota Finance")]
    public LospusicCougar ToyotaFinance { get; set; }

    [JsonProperty("Impact Financial")]
    public LospusicCougar ImpactFinancial { get; set; }

    [JsonProperty("Yamaha Motor Finance")]
    public LospusicCougar YamahaMotorFinance { get; set; }

    [JsonProperty("Home Loan")]
    public LospusicCougar HomeLoan { get; set; }

    [JsonProperty("Generic")]
    public LospusicCougar Generic { get; set; }
  }

  public partial class FairGoFinance
  {
    [JsonProperty("transactionCount")]
    public long TransactionCount { get; set; }

    [JsonProperty("totalValue")]
    public long TotalValue { get; set; }

    [JsonProperty("monthAvg")]
    public double MonthAvg { get; set; }

    [JsonProperty("minValue")]
    public double MinValue { get; set; }

    [JsonProperty("maxValue")]
    public double MaxValue { get; set; }

    [JsonProperty("firstTransaction")]
    public string FirstTransaction { get; set; }

    [JsonProperty("lastTransaction")]
    public string LastTransaction { get; set; }

    [JsonProperty("period")]
    public double Period { get; set; }

    [JsonProperty("periodIsRegular")]
    public bool PeriodIsRegular { get; set; }

    [JsonProperty("transactions")]
    public List<Detail> Transactions { get; set; }
  }

  public partial class Detail
  {
    [JsonProperty("dateObj")]
    public DateObj DateObj { get; set; }

    [JsonProperty("date")]
    public string Date { get; set; }

    [JsonProperty("text")]
    public string Text { get; set; }

    [JsonProperty("notes")]
    public object Notes { get; set; }

    [JsonProperty("transactionHash")]
    public object TransactionHash { get; set; }

    [JsonProperty("hashText")]
    public object HashText { get; set; }

    [JsonProperty("amount")]
    public double Amount { get; set; }

    [JsonProperty("type")]
    public TypeEnum Type { get; set; }

    [JsonProperty("balance")]
    public double Balance { get; set; }

    [JsonProperty("tags")]
    public List<string> Tags { get; set; }
  }

  public partial class DateObj
  {
    [JsonProperty("date")]
    public DateTimeOffset Date { get; set; }

    [JsonProperty("timezone_type")]
    public int TimezoneType { get; set; }

    [JsonProperty("timezone")]
    public string Timezone { get; set; }
  }

  public partial class OtherDebits
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("Paygate Payments")]
    public LospusicCougar PaygatePayments { get; set; }

    [JsonProperty("SPER")]
    public LospusicCougar Sper { get; set; }

    [JsonProperty("Certegy")]
    public LospusicCougar Certegy { get; set; }
  }

  public partial class Rent
  {
    [JsonProperty("total")]
    public Total Total { get; set; }

    [JsonProperty("Rent")]
    public LospusicCougar RentRent { get; set; }
  }

  public partial class DayEndBalance
  {
    [JsonProperty("date")]
    public DateTimeOffset Date { get; set; }

    [JsonProperty("balance")]
    public double Balance { get; set; }
  }

  public enum Tag { Loans, ShortTermLender };

  public enum TypeEnum { Credit, Debit };

  public partial struct Value
  {
    public long? Integer;
    public string String;

    public bool IsNull => Integer == null && String == null;
  }

  public partial class FetchAllResponse
  {
    public static FetchAllResponse FromJson(string json) => JsonConvert.DeserializeObject<FetchAllResponse>(json, Vita.Domain.BankStatements.Models.Converter.Settings);
  }

  public static class Serialize
  {
    public static string ToJson(this FetchAllResponse self) => JsonConvert.SerializeObject(self, Vita.Domain.BankStatements.Models.Converter.Settings);
  }

  internal static class Converter
  {
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
      DateParseHandling = DateParseHandling.None,
      Converters = {
                new TypeEnumConverter(),
                new ValueConverter(),
                new TagConverter(),
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
  }

  internal class TypeEnumConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(TypeEnum) || t == typeof(TypeEnum?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) return null;
      var value = serializer.Deserialize<string>(reader);
      switch (value)
      {
        case "Credit":
          return TypeEnum.Credit;
        case "Debit":
          return TypeEnum.Debit;
      }
      throw new Exception("Cannot unmarshal type TypeEnum");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      var value = (TypeEnum)untypedValue;
      switch (value)
      {
        case TypeEnum.Credit:
          serializer.Serialize(writer, "Credit"); return;
        case TypeEnum.Debit:
          serializer.Serialize(writer, "Debit"); return;
      }
      throw new Exception("Cannot marshal type TypeEnum");
    }
  }

  internal class ValueConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(Value) || t == typeof(Value?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) return null;
      switch (reader.TokenType)
      {
        case JsonToken.Integer:
          var integerValue = serializer.Deserialize<long>(reader);
          return new Value { Integer = integerValue };
        case JsonToken.String:
        case JsonToken.Date:
          var stringValue = serializer.Deserialize<string>(reader);
          return new Value { String = stringValue };
      }
      throw new Exception("Cannot unmarshal type Value");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      var value = (Value)untypedValue;
      if (value.Integer != null)
      {
        serializer.Serialize(writer, value.Integer); return;
      }
      if (value.String != null)
      {
        serializer.Serialize(writer, value.String); return;
      }
      throw new Exception("Cannot marshal type Value");
    }
  }

  internal class TagConverter : JsonConverter
  {
    public override bool CanConvert(Type t) => t == typeof(Tag) || t == typeof(Tag?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
      if (reader.TokenType == JsonToken.Null) return null;
      var value = serializer.Deserialize<string>(reader);
      switch (value)
      {
        case "Loans":
          return Tag.Loans;
        case "Short Term Lender":
          return Tag.ShortTermLender;
      }
      throw new Exception("Cannot unmarshal type Tag");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
      var value = (Tag)untypedValue;
      switch (value)
      {
        case Tag.Loans:
          serializer.Serialize(writer, "Loans"); return;
        case Tag.ShortTermLender:
          serializer.Serialize(writer, "Short Term Lender"); return;
      }
      throw new Exception("Cannot marshal type Tag");
    }
  }
}
