using System;
using System.Reflection;
using SpreadsheetGear;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Services.TextClassifiers.SpreadSheets
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
      // var path = @"C:\dev\FairGo.Pricing\source\FairGo.Pricing\Spreadsheets\Products_and_Pricing.xlsm";
      //Workbook = ExcelFile.Load(path);

      //foreach (var name in assembly.GetManifestResourceNames())
      //{
      //  Console.WriteLine(name);
      //}

      if (string.IsNullOrWhiteSpace(resourcename))
      {
        resourcename = "Vita.Domain.Services.TextClassifiers.SpreadSheets.keywords.xlsx";
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