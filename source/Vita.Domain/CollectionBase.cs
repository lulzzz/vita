using System;
using Serilog;

namespace Vita.Domain
{
  public abstract class CollectionBase
  {

    public virtual void LogVerbose(string message)
    {
      Log.Verbose("{text} {message}", GetType().Name, message);
    }


    public virtual void LogDebug(string message)
    {
      Log.Debug("{text} {message}", GetType().Name, message);
    }

    public virtual void LogInfo(string message)
    {
      Log.Information("{text} {message}", GetType().Name, message);
    }

    public virtual void LogWarn(string message)
    {
      Log.Warning("{text} {message}", GetType().Name, message);
    }

    public virtual void LogWarn(Exception e, string message)
    {
      Log.Warning(e, "{text} {message}", GetType().Name, message);
    }

    public virtual void LogError(Exception e, string message)
    {
      Log.Error(e,"{text} {message}", GetType().Name, message);
    }

    public bool Export = false;
  }
}