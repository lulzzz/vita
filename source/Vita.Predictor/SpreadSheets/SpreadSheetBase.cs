using System;
using System.Reflection;
using SpreadsheetGear;
using Vita.Domain.Infrastructure;

namespace Vita.Predictor.SpreadSheets
{
  public abstract class SpreadSheetBase
  {
    public static bool LicenseSet;
    private static readonly object Keylock = new object();

    protected SpreadSheetBase()
    {
      lock (Keylock)
      {
        if (!LicenseSet)
        {
          string license = SecretMan.Get("SpreadsheetGearLicense");
          Factory.SetSignedLicense(license);
          LicenseSet = true;
        }
      }
    }

    public IWorkbook Workbook { get; protected set; }

    public void LoadExcelSheet(string resourcename = null)
    {
      var assembly = Assembly.GetAssembly(typeof(KeywordsSpreadsheet));

      if (string.IsNullOrWhiteSpace(resourcename))
      {
        resourcename = "Vita.Predictor.SpreadSheets.keywords.xlsx";
      }

      using (var stream = assembly.GetManifestResourceStream(resourcename))
      {
        if (stream == null) throw new ApplicationException("Keywords spreadsheet not loaded");
        Workbook = Factory.GetWorkbookSet().Workbooks.OpenFromStream(stream);
        Workbook.WorkbookSet.Calculation = Calculation.Manual;
        Workbook.WorkbookSet.BackgroundCalculation = false;
        Workbook.WorkbookSet.CalculationOnDemand = false;
      }
    }
  }
}