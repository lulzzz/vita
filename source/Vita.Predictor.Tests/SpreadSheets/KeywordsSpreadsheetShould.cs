using System.Linq;
using ExtensionMinder;
using FluentAssertions;
using Vita.Contracts;
using Vita.Predictor.SpreadSheets;
using Xunit;

namespace Vita.Predictor.Tests.SpreadSheets
{
    public class KeywordsSpreadsheetShould
    {
      [Fact]
      public void LoadCategoryKeywords_loads_spreadsheet_data()
      {
        var kws = new KeywordsSpreadsheet();
        Assert.NotNull(kws.Workbook);

        var ck = kws.LoadData();
        ck.Should().NotBeEmpty();
        kws.CategoryTypes.GetDuplicates().Should().BeEmpty();
        var cats = ck.Select(x => x.CategoryType);
        var subcats = ck.Select(x => x.SubCategory);

        var dupes = subcats.GetDuplicates().ToList();

        Assert.Empty(dupes);

        cats.Should().Contain(CategoryType.BankingFinance);
        subcats.Should().Contain("Fees");

        var item = ck.Single(x => x.CategoryType == CategoryType.Entertainment && x.SubCategory == "Movies");
        string expect = "cinemas";
        item.Keywords.Should().Contain(expect.ToLowerInvariant());


      }
    }
}
