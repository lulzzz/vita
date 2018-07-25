using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LiteDB;
using Serilog;
using Vita.Contracts;

namespace Vita.Domain.Infrastructure
{
  public class Repository<T> : IRepository<T> where T : class
  {
    private static readonly object Keylock = new object();

    private string GetPath()
    {
      if (Environment.MachineName.ToUpperInvariant() == "EARTH")
        return $@"D:\Dropbox\Dropbox (Personal)\Red-Trout\chargeid\data\{typeof(T).Name}.db";

      if (Environment.MachineName.ToUpperInvariant() == "SATURN")
        return $@"c:\sql\{typeof(T).Name}.db";

      var location = System.Reflection.Assembly.GetEntryAssembly().Location;
      var directory = System.IO.Path.GetDirectoryName(location);

      string file = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
      Console.WriteLine(file);
      if (!File.Exists(file))
      {
        file = Directory.GetFiles(directory,"*.*",SearchOption.AllDirectories).Single(x => x.Contains($"{typeof(T).Name}.db"));
      }

      if (!File.Exists(file))
      {
        Log.Fatal("Repository file missing for {a} not found at {file}", typeof(T), file);
        throw new FileNotFoundException(file);
      }

      return file;

    }

    public Guid Insert(T entity)
    {
      lock (Keylock)
      {
        using (var repo = new LiteRepository(new LiteDatabase(GetPath())))
        {
          var id = repo.Insert<T>(entity);
          return id.AsGuid;
        }
      }
    }
    /// <summary>
    /// true if insert
    /// false if update
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public bool Save(T entity)
    {
      lock (Keylock)
      {
        using (var repo = new LiteRepository(new LiteDatabase(GetPath())))
        {
          var result = repo.Upsert<T>(entity);
          return result;
        }
      }
    }

    public void Delete(Guid id)
    {
      lock (Keylock)
      {
        using (var repo = new LiteRepository(new LiteDatabase(GetPath()), true))
        {
          var bs = new BsonValue(id);
          repo.Delete<T>(bs);
        }
      }
    }

    public IEnumerable<T> GetAll()
    {
      lock (Keylock)
      {
        using (var repo = new LiteRepository(new LiteDatabase(GetPath())))
        {
          var found =  repo.Database.GetCollection<T>().FindAll();
          Trace.WriteLine($"GetAll() {found.Count()}");
          return found;
        }
      }     
    }

    public T GetById(Guid id)
    {
      lock (Keylock)
      {
        var bs = new BsonValue(id);
        using (var repo = new LiteRepository(new LiteDatabase(GetPath())))
        {
          return repo.Database.GetCollection<T>().FindById(bs);
        }
      }
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = 2147483647)
    {
      lock (Keylock)
      {
        using (var repo = new LiteRepository(new LiteDatabase(GetPath()), true))
        {
          return repo.Database.GetCollection<T>().Find(predicate, skip, limit);
        }
      }
     
    }

    public bool EnsureIndex<K>(Expression<Func<T, K>> property, bool unique = true)
    {
      lock (Keylock)
      {
        using (var repo = new LiteRepository(new LiteDatabase(GetPath()), true))
        {
          return repo.Database.GetCollection<T>().EnsureIndex<K>(property, unique);
        }
      }
    }

  }
}