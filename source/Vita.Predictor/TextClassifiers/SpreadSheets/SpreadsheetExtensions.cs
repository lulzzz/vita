using System;
using Serilog;

namespace Vita.Predictor.TextClassifiers.SpreadSheets
{
    public static class SpreadsheetExtensions
    {
    public static double ToNullDouble(this object obj)
    {
      var value = Convert.ToString(obj);
      // Excel gives u "NA"
      if (string.IsNullOrWhiteSpace(value) || value == "NA" || value == "#NA") return default(double);
      if (double.TryParse(value, out var number))
      {
        return number;
      }

      throw new ArgumentException($"Invalid double: {value}");
    }

    public static int ToInt(this object obj)
    {
      var value = Convert.ToString(obj);
      // Excel gives u "NA"
      if (string.IsNullOrWhiteSpace(value) || value == "NA" || value == "#NA") return default(int);
      if (int.TryParse(value, out int number))
      {
        return number;
      }

      throw new ArgumentException($"Invalid int: {value}");
    }

    public static int? ToNullableInt(this object obj)
    {
      var value = Convert.ToString(obj);
      // Excel gives u "NA"
      if (string.IsNullOrWhiteSpace(value) || value == "NA" || value == "#NA") return null;

      if (int.TryParse(value, out int number))
      {
        return number;
      }

      throw new ArgumentException($"Invalid int: {value}");
    }


    public static bool? ToNullBoolean(this object obj)
    {
      var value = Convert.ToString(obj);
      // Excel gives u "NA"
      if (string.IsNullOrWhiteSpace(value) || value == "NA" || value == "#NA") return null;
      if (value.ToLowerInvariant() == "yes") return true;
      if (value.ToLowerInvariant() == "no") return false;

      return Convert.ToBoolean(value);
    }

    public static bool ToBoolean(this object obj)
    {
      var value = Convert.ToString(obj);
      return ToBoolean(value);
    }

    public static string ToYesNo(this bool? arg)
    {
      if (!arg.HasValue) return string.Empty;

      return arg == true ? "Yes" : "No";
    }

    public static bool ToBoolean(this string value)
    {
      // Excel gives u "NA"
      if (string.IsNullOrWhiteSpace(value) || value == "NA") return false;

      if (value.ToLowerInvariant() == "yes") return true;
      if (value.ToLowerInvariant() == "no") return false;
      if (value.ToLowerInvariant() == "1") return true;
      if (value.ToLowerInvariant() == "0") return false;
      if (value.ToLowerInvariant() == "na") return false;

      try
      {
        return Convert.ToBoolean(value);
      }
      catch (Exception e)
      {
        Log.Warning(e, $"Boolean conversion failed: {value}");
        throw;
      }

    }
  }
}
