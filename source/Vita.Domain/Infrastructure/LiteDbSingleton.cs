using LiteDB;

namespace Vita.Domain.Infrastructure
{
  public sealed class LiteDbSingleton
  {
    public LiteDatabase Database { get; set; }
    public static LiteDbSingleton Instance { get; } = new LiteDbSingleton();

    // Explicit static constructor to tell C# compiler
    // not to mark type as beforefieldinit
    static LiteDbSingleton()
    {
    }

    private LiteDbSingleton()
    {
      Database = new LiteDatabase(@"D:\Dropbox\Dropbox (Personal)\Red-Trout\chargeid\data\vita.db");
    }

  }
}
