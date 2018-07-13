using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using ExtensionMinder;
using Vita.Contracts;

namespace Vita.Domain.Services.TextClassifiers.SpreadSheets
{
  public static class CompanySpreadsheet
  {
    public static IEnumerable<Company> Import()
    {
      var sr = new StreamReader(@"D:\Dropbox\Dropbox (Personal)\Red-Trout\chargeid\data\COMPANY.csv");
      var csv = new CsvReader(sr);

      csv.Configuration.Delimiter = "\t";
      csv.Configuration.HeaderValidated = null;
      csv.Configuration.HasHeaderRecord = true;
      csv.Configuration.IgnoreBlankLines = true;      
      csv.Configuration.RegisterClassMap<AsicCompanyMap>();
      var records = csv.GetRecords<Company>().ToList();
      return records.Select(x=>x.TrimAllStrings());
    }
  }

  public sealed class AsicCompanyMap : ClassMap<Company>
  {
    public AsicCompanyMap()
    {
      var count = 1;
      Map(m => m.CompanyName).Index(0);
      Map(m => m.AustralianCompanyNumber).Index(count++);
      Map(m => m.CompanyType).Index(count++);
      Map(m => m.CompanyClass).Index(count++);
      Map(m => m.SubClass).Index(count++);
      Map(m => m.Status).Index(count++);
      Map(m => m.DateOfRegistration).Index(count++);
      Map(m => m.PreviousStateOfRegistration).Index(count++);
      Map(m => m.StateOfRegistrationNumber).Index(count++);
      Map(m => m.ModifiedSinceLastReport).Index(count++);
      Map(m => m.CurrentNameIndicator).Index(count++);
      Map(m => m.AustralianBusinessNumber).Index(count++);
      Map(m => m.CurrentName).Index(count++);
      Map(m => m.CurrentNameStartDate).Index(count++);
      Map(m => m.CompanyCurrentInd).Index(count++);
      Map(m => m.CompanyCurrentName).Index(count++);
      Map(m => m.CompanyCurrentNameStartDt).Index(count++);
      Map(m => m.CompanyModifiedSinceLast).Index(count++);
    }
  }
}