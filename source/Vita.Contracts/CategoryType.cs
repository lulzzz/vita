using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Vita.Contracts
{
  [JsonConverter(typeof(StringEnumConverter))]
  public enum CategoryType
  {
    [Description("Banking & Finance")] BankingFinance,
    [Description("Entertainment")] Entertainment,
    [Description("Food & Drinks")] FoodDrinks,
    [Description("Groceries")] Groceries,
    [Description("Health & Beauty")] HealthBeauty,
    [Description("Holiday & Travel")] HolidayTravel,
    [Description("Home")] Home,
    [Description("Household Utilities")] HouseholdUtilities,
    [Description("Income")] Income,
    [Description("Insurance")] Insurance,
    [Description("Kids")] Kids,
    [Description("Miscellaneous")] Miscellaneous,
    [Description("Shopping")] Shopping,
    [Description("Transferring Money")] TransferringMoney,
    [Description("Transport")] Transport,
    [Description("Uncategorised")] Uncategorised,
    [Description("Work & Study")] WorkStudy,
  }
}