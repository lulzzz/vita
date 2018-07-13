using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using CsvHelper;
using ExtensionMinder;
using Serilog;
using Vita.Domain.Services.TextClassifiers;
using Vita.Domain.Services.TextClassifiers.SpreadSheets;

namespace Vita.DataImporter
{
  public static class Util
  {
    public static IEnumerable<T> ImportCsv<T>(string filename, bool hasHeaderRecord = false)
    {
      if (!File.Exists(filename)) throw new FileNotFoundException(filename);
      using (var sr = new StreamReader(filename))
      {
        var csv = new CsvReader(sr);
        csv.Configuration.HasHeaderRecord = hasHeaderRecord;
        var records = csv.GetRecords<T>();
        foreach (var rec in records) yield return rec.TrimAllStrings();
      }
    }

    public static void WaitFor(int secs = 1)
    {
      Thread.SpinWait(TimeSpan.FromSeconds(secs).Seconds);
    }

    public static void WriteOutKeywords()
    {
      var localities = new LocalitiesSpreadsheet().LoadData();
      var seen = new List<string>();
      //  var keywordClassifiers = new KeywordsSpreadsheet().LoadData().ToList();
      //var companies = CompanyImporter.Import().ToList();
      const string folder = @"C:\dev\vita\data\output";
      Directory.EnumerateFiles(folder).Each(File.Delete);
      var sss = new KeywordsSpreadsheet();
      var data = sss.LoadData();
      foreach (var item in data)
      {
        var cleaned = new List<string>();
        var words = item.Keywords.Distinct();
        var wordsArray = words as string[] ?? words.ToArray();
        foreach (var word in wordsArray.Select(x => x.ToLowerInvariant()))
          try
          {
            var mutate = word;
            var finds = localities.Where(x =>
              word.Contains(x.Suburb.ToLowerInvariant()) || word.Contains(x.Postcode));
            foreach (var found in finds)
            {
              if (!string.IsNullOrWhiteSpace(found.Suburb))
                mutate = mutate.Replace(found.Suburb.ToLowerInvariant(), string.Empty);
              if (!string.IsNullOrWhiteSpace(found.Postcode))
                mutate = mutate.Replace(found.Postcode.ToLowerInvariant(), string.Empty);
            }

            mutate = TextUtil.CleanWord(mutate);
            cleaned.Add(mutate);
          }
          catch (Exception e)
          {
            Log.Warning(e, "word {w}", word);
          }


        var path = Path.Combine(folder, item.SubCategory + ".csv");
        if (!seen.Contains(path))
          seen.Add(path);
        else
          throw new ApplicationException(path);

        using (TextWriter writer = new StreamWriter(path))
        {
          var csv = new CsvWriter(writer);
          foreach (var word in cleaned.Distinct().OrderBy(x => x))
          {
            csv.WriteField(word.ToLowerInvariant());
            csv.NextRecord();
          }

          writer.Flush();
        }
      }
    }
  }
}