using System;
using System.IO;
using System.Reflection;

namespace Vita.Predictor
{
  public static class PredictionModelWrapper
  {
    public static string GetAppPath()
    {
      var local = @"c:\dev\vita\data\";

      if (Directory.Exists(local)) return local;

      return Directory.Exists(local) ? local : AppDomain.CurrentDomain.BaseDirectory;
    }

    public static string GetFilePath(string filename, bool scanChildFolders = true, bool throwIfNotFound = true)
    {
      if (scanChildFolders)
      {
        foreach (var file in Directory.EnumerateFiles(GetAppPath(), filename, SearchOption.AllDirectories))
          if (file.Contains(filename))
            return file;
      }
      else
      {
        return Path.Combine(GetAppPath(), filename);
      }

      if (throwIfNotFound) throw new FileNotFoundException(filename);

      return null;
    }

    //private static string AppPath => @"c:\dev\vita\data\";
    public static string Model1Path => GetFilePath("vita-model-1.zip");

    public static Stream GetModel()
    {
      var assembly = Assembly.GetExecutingAssembly();
      var resourceName = "Vita.Predictor.vita-model-1.zip";

      var names = assembly.GetManifestResourceNames();
      foreach (var name in names) Console.WriteLine(name);
      return assembly.GetManifestResourceStream(resourceName);
    }
  }
}