using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Vita.Domain.BankStatements;

namespace Vita.Domain.Infrastructure.Importers
{
  public static class PocketBookImporter
  {
    public static IEnumerable<PocketBook> Import(string path)
    {
      using (StreamReader sr = new StreamReader(path))
      using (var csv = new CsvReader(sr))
      {
        //string sss = sr.ReadToEnd();
        csv.Configuration.Delimiter = ",";
        csv.Configuration.HeaderValidated = null;
        csv.Configuration.HasHeaderRecord = true;
        csv.Configuration.IgnoreBlankLines = true;
        // csv.Configuration.MissingFieldFound = null;
        csv.Configuration.TrimOptions = TrimOptions.Trim;
        //csv.Configuration.TypeConverterCache.<string>(new SubCategoryTypeConverter());

        csv.Configuration.RegisterClassMap<PocketBookMap>();

        return csv.GetRecords<PocketBook>().ToList();

      }
    }
  }

  
  public sealed class PocketBookMap : ClassMap<PocketBook>
  {
    public PocketBookMap()
    {
      ///date	description	category	amount	notes	tags	bank	accountname	accountnumber

      Map(m => m.Date).Index(0);
      Map(m => m.Description).Index(1);
      Map(m => m.Category).Index(2);
      Map(m => m.Amount).Index(3);
      Map(m => m.Notes).Index(4);
      Map(m => m.Tags).Index(5);
      Map(m => m.Bank).Index(6);
      Map(m => m.AccountName).Index(7);
      Map(m => m.AccountNumber).Index(8);

    }
  }
}