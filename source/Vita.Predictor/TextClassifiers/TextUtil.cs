using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Vita.Domain.Infrastructure;

namespace Vita.Predictor.TextClassifiers
{
  public static class TextUtil
  {
    public static readonly string[] ReplaceWords = new string[]
    {
      "wa",
      "nsw",
      "qld",
      "vic",
      "act",
      "tas",
      "nt",
      "sa",
      "sydney",
      "albury",
      "armidale",
      "bathurst",
      "broken hill",
      "cessnock",
      "coffs harbour",
      "dubbo",
      "gosford",
      "goulburn",
      "grafton",
      "griffith",
      "lake macquarie",
      "lismore",
      "maitland",
      "newcastle",
      "nowra",
      "orange",
      "port macquarie",
      "queanbeyan",
      "tamworth",
      "tweed heads",
      "wagga wagga",
      "wollongong",
      "wyong",

      "darwin",
      "alice springs",
      "katherine",
      "palmerston",

      "brisbane",
      "bundaberg",
      "cairns",
      "charters towers",
      "gladstone",
      "gold coast",
      "gympie",
      "hervey bay",
      "ipswich",
      "logan city",
      "mackay",
      "maryborough",
      "mount isa",
      "nambour",
      "redcliffe",
      "rockhampton",
      "sunshine coast",
      "thuringowa",
      "toowoomba",
      "townsville",

      "adelaide",
      "gladstone",
      "mount gambier",
      "murray bridge",
      "port augusta",
      "port pirie",
      "port lincoln",
      "victor harbor",
      "whyalla",

      "hobart",
      "burnie",
      "clarence",
      "devonport",
      "glenorchy",
      "launceston",

      "melbourne",
      "benalla",
      "ballarat",
      "bendigo",
      "geelong",
      "latrobe city",
      "mildura",
      "shepparton",
      "swan hill",
      "wangaratta",
      "warrnambool",
      "wodonga",

      "perth",
      "albany",
      "broome",
      "bunbury",
      "geraldton",
      "fremantle",
      "kalgoorlie",
      "mandurah",
      "port hedland"
    };

    public static string CleanWord(string mutate)
    {
      if (string.IsNullOrWhiteSpace(mutate)) return string.Empty;

      mutate = mutate.Trim();

      foreach (var word in ReplaceWords)
      {
        string pattern = $@"\b{word}\b";
        mutate = Regex.Replace(mutate, pattern, " ");
      }
    
      mutate = mutate.Replace("  ", " ");
      return mutate;
    }


    public static DateTime? ParseAuDate(string date)
    {
      var dt = DateTime.MinValue;
      if (date.TryParseDateTime(DateTimeUtil.DateTimeFormat.UkDate, out dt)) return dt;

      return Convert.ToDateTime(date, DateTimeFormatInfo.CurrentInfo);
    }
  }
}