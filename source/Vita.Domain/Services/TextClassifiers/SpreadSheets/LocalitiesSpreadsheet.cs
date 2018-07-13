using System.Collections.Generic;
using ExtensionMinder;
using Vita.Contracts;

namespace Vita.Domain.Services.TextClassifiers.SpreadSheets
{
  public class LocalitiesSpreadsheet : SpreadSheetBase
  {

    private readonly List<Locality> _localities = new List<Locality>();

    public LocalitiesSpreadsheet()
    {
      LoadExcelSheet("Vita.Domain.Services.TextClassifiers.SpreadSheets.localities.xlsx");
    }

    public IEnumerable<Locality> LoadData()
    {
      var sheet = Workbook.Worksheets["suburbs"];
      for (int row = 2; row < sheet.Range.RowCount; row++)
      {
        var local = new Locality
        {
          Postcode = sheet.Cells["A" + row].Text,
          Suburb = sheet.Cells["B" + row].Text,
          AustralianState = sheet.Cells["C" + row].Text.ToEnum<AustralianState>(),
          Latitude = sheet.Cells["D" + row].Text.ToNullDouble(),
          Longitude = sheet.Cells["E" + row].Text.ToNullDouble()
        };
        _localities.Add(local);
      }

      return _localities;
    }
  }
}