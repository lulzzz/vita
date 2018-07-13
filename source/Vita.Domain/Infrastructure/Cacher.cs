using System;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Vita.Domain.Infrastructure
{
  public static class Cacher
  {
    private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1);

    public static async Task<T> GetAsync<T>(string cacheKey, Func<Task<T>> setFunc, DateTimeOffset? expiry = null) where T : class
    {
      await SemaphoreSlim.WaitAsync();
      try
      {
        var result = Get<T>(cacheKey);
        if (result != null)
        {
          Log.Verbose($"{cacheKey} retrieved from cache");
          return result;
        }

        Log.Verbose($"Retrieving {cacheKey}");
        result = await setFunc().ConfigureAwait(false);

        Log.Verbose($"Saving retrieved {cacheKey} to cache");
        Set(cacheKey, result, expiry);

        return result;
      }
      finally
      {
        SemaphoreSlim.Release();
      }

    }

    public static T GetOrSet<T>(string cacheKey, Func<T> retrieveFunc, DateTimeOffset? expiry = null) where T : class
    {
      SemaphoreSlim.Wait();
      try
      {
        var cached = Get<T>(cacheKey);
        if (cached != null)
        {
          Log.Verbose($"{cacheKey} retrieved from cache");
          return cached;
        }

        Log.Verbose($"Retrieving {cacheKey}");
        var result = retrieveFunc();

        Log.Verbose($"Saving retrieved {cacheKey} to cache");
        Set(cacheKey, result, expiry);

        return result;
      }
      finally
      {
        SemaphoreSlim.Release();
      }

    }

    public static void Set<T>(string cacheKey, T item, DateTimeOffset? expiry = null)
    {
      System.Runtime.Caching.MemoryCache.Default.Add(cacheKey, item, expiry ?? DateTimeOffset.MaxValue);
    }

    public static T Get<T>(string cacheKey) where T : class
    {
      return System.Runtime.Caching.MemoryCache.Default.Get(cacheKey) as T;;
    }

    public static bool Exists(string cacheKey)
    {
      return System.Runtime.Caching.MemoryCache.Default.Contains(cacheKey);
    }

    public static void Remove(string cacheKey)
    {
      System.Runtime.Caching.MemoryCache.Default.Remove(cacheKey);
    }
  }
}