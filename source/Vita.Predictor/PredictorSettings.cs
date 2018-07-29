using System;
using System.IO;

namespace Vita.Predictor
{
    public static class PredictorSettings
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
    }
}