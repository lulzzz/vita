using FluentAssertions;
using Vita.Contracts;

using Vita.Domain.Infrastructure;
using Xunit;

namespace Vita.Domain.Tests.Infrastructure
{
  public class CategoryTypeConverterShould
  {
    [Theory]
    [InlineData("Groceries - Supermarkets",CategoryType.Groceries)]
    [InlineData("Transferring Money - Other Transferring Money", CategoryType.TransferringMoney)]
    [InlineData("Food & Drinks - Bars & Pubs", CategoryType.FoodDrinks)]
    [InlineData("Uncategorised", CategoryType.Uncategorised)]
    [InlineData("Banking & Finance - Loan Repayments", CategoryType.BankingFinance)]
    [InlineData("Insurance", CategoryType.Insurance)]
    [InlineData("aaaaaaa", CategoryType.Uncategorised)]
    public void From_place_enum_match(string text, CategoryType expected)
    {
      CategoryTypeConverter.FromPlace(text).Should().Be(expected);
    }


    [Theory]
    [InlineData(SubCategories.Groceries.Supermarkets,CategoryType.Groceries)]
    [InlineData(SubCategories.Groceries.OtherGroceries,CategoryType.Groceries)]
    [InlineData(SubCategories.FoodDrinks.BarsPubs,CategoryType.FoodDrinks)]
    [InlineData(SubCategories.HolidayTravel.OtherTravel,CategoryType.HolidayTravel)]
    [InlineData(SubCategories.Miscellaneous.Other,CategoryType.Miscellaneous)]
    [InlineData("aaaaaaa", CategoryType.Uncategorised)]
    public void From_subcategory_enum_match(string text, CategoryType expected)
    {
      CategoryTypeConverter.FromSubcategory(text).Should().Be(expected);
    }
    
  }
}