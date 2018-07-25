using System;
using System.Linq;

namespace Vita.Predictor.TextClassifiers
{
  /// <summary>
  /// Gibberish, alternatively jibberish, jibber-jabber, or gobbledygook, is language that is (or appears to be) nonsense. It may include speech sounds that are not actual words,[1] or language games and specialized jargon that seems nonsensical to outsiders.[2] Gibberish should not be confused with literary nonsense such as that used in the poem "Jabberwocky" by Lewis Carroll.[citation needed]
  /// </summary>
  public static class Gibberish
    {
      public static bool IsGibberish(string word)
      {
        if (string.IsNullOrWhiteSpace(word)) return true;

        try
        {
          var chars = word.ToCharArray();
          if (chars == null || !chars.Any()) return true;

          var numbers = chars.Count(Char.IsDigit);
          var letters = chars.Count(x => !Char.IsDigit(x));

          if (numbers == 4 && letters == 0) return false; //postcode

          return numbers > letters;
      }
        catch (Exception e)
        {
          Console.WriteLine(e);
          return true;
        }
      
      }
    }
}
