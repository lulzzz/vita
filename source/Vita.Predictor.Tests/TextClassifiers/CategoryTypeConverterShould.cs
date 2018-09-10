using FluentAssertions;
using Vita.Contracts;
using Vita.Domain.Infrastructure;
using Vita.Predictor.TextClassifiers;
using Xunit;

namespace Vita.Predictor.Tests.TextClassifiers
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
    public void Convert_from_enum_match(string text, CategoryType expected)
    {
      CategoryTypeConverter.FromPlace(text).Should().Be(expected);
    }
  }
}