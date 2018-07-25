using System.Linq;
using FluentAssertions;
using Vita.Predictor.TextClassifiers.SpreadSheets;
using Xunit;

namespace Vita.Predictor.Tests.TextClassifiers.SpreadSheets
{
  public class SuburbsSpreadsheetShould
  {
    [Fact]
    public void LoadData_should_read_excel_data()
    {
      var spreadsheet = new LocalitiesSpreadsheet();
      var results = spreadsheet.LoadData();
      Assert.NotEmpty(results);

      var data = results.Single(x => x.Postcode == "6023");
      data.Suburb.Should().Be("DUNCRAIG");
       
    }
  }
}