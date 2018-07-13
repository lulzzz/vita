using System;
using System.Collections.Generic;
using System.Linq;
using ExtensionMinder;
using Serilog;
using SpreadsheetGear;
using Vita.Contracts;

namespace Vita.Domain.Services.TextClassifiers.SpreadSheets
{
  public class KeywordsSpreadsheet : SpreadSheetBase
  {
    private readonly IList<Contracts.Classifier> _identifiers = new List<Contracts.Classifier>();
    public IEnumerable<CategoryType> CategoryTypes { get; set; } = CategoryType.FoodDrinks.GetAllItems<CategoryType>();
    public string[] Letters = new[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "L", "M", "N"}; // ballz -- done fast - cycle over columns

    public KeywordsSpreadsheet()
    {
      LoadExcelSheet();
    }

    public IEnumerable<Classifier> LoadData()
    {
      Log.Debug("spreadsheet KeywordsSpreadsheet try {method}", "LoadData");
      var count = 0;
      foreach (var cat in CategoryTypes.Where(x => x != CategoryType.Uncategorised).Distinct())
      {
        Log.Verbose("spreadsheet LoadCategoryKeywords try {cat}", cat);

        count++;
        if (count > 20) throw new ApplicationException();

        var sheet = Workbook.Worksheets[cat.ToString()];
        if (sheet == null) throw new ArgumentException(cat.ToString());
        Log.Debug("spreadsheet LoadCategoryKeywords ok {cat}", cat);

        foreach (var letter in Letters)
        {
          var keywords = GetSubCategoryWithKeywords(sheet, letter);

          var identifier = new Contracts.Classifier
          {
            Id = Guid.NewGuid(),
            CategoryType = cat,
            SubCategory = keywords.Item1.Clean().Replace(" ",string.Empty).Trim()
          };

          foreach (var data in keywords.Item2)
          {
            identifier.AddKeyword(data);
            Log.Verbose("classifier {s} {k}", identifier.SubCategory, data);
          }

          _identifiers.Add(identifier);
        }

      }

      //Debug.Assert(_identifiers.Select(x => x.SubCategory).Count() < 80);
      return _identifiers.Where(x => !string.IsNullOrWhiteSpace(x.SubCategory));
    }

    private Tuple<string, List<string>> GetSubCategoryWithKeywords(IWorksheet sheet, string colReference)
    {
      var subcatName = string.Empty;
      var keywords = new List<string>();
      
     foreach(IRange column in sheet.Cells[$"{colReference}1"])
     {
        subcatName = Convert.ToString(column?.Value);
       
        var entireColumn = column?.EntireColumn;

        if (entireColumn?.Rows != null)
        {
          keywords.AddRange(from IRange row in entireColumn?.Rows
            where row?.Value != null
            select row.Value.ToString() into data
            where !string.IsNullOrWhiteSpace(data)
            select data);
        }
      }

      return new Tuple<string, List<string>>(subcatName, keywords);
    }
  }
}