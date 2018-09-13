using FluentAssertions;
using Vita.Contracts;
using Vita.Contracts.SubCategories;
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
    [InlineData(Categories.Groceries.Supermarkets,CategoryType.Groceries)]
    [InlineData(Categories.Groceries.OtherGroceries,CategoryType.Groceries)]
    [InlineData(Categories.FoodDrinks.BarsPubs,CategoryType.FoodDrinks)]
    [InlineData(Categories.HolidayTravel.OtherTravel,CategoryType.HolidayTravel)]
    [InlineData(Categories.Miscellaneous.Other,CategoryType.Miscellaneous)]
    [InlineData("aaaaaaa", CategoryType.Uncategorised)]
    public void From_subcategory_enum_match(string text, CategoryType expected)
    {
      CategoryTypeConverter.FromSubcategory(text).Should().Be(expected);
    }
    
  }
}