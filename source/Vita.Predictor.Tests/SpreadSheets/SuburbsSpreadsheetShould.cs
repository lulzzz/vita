using System.Linq;
using FluentAssertions;
using Vita.Contracts;
using Vita.Predictor.SpreadSheets;
using Xunit;

namespace Vita.Predictor.Tests.SpreadSheets
{
  public class SuburbsSpreadsheetShould
  {
    [Fact]
    public void LoadData_should_read_excel_data()
    {
      var spreadsheet = new LocalitiesSpreadsheet();
      var results = spreadsheet.LoadData();
        var collection = results as Locality[] ?? results.ToArray();
        Assert.NotEmpty(collection);

      var data = collection.Single(x => x.Postcode == "6023");
      data.Suburb.Should().Be("DUNCRAIG");
       
    }
  }
}