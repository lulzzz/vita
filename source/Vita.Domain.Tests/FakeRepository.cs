using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Vita.Contracts;
using Vita.Domain.Infrastructure;

namespace Vita.Domain.Tests
{
  public class FakeRepository<T> : IRepository<T>
  {
    private IList<T> _list = new List<T>();

    public void Load(IEnumerable<T> list)
    {
      _list = list.ToList();
    }

    public void Add(T item)
    {
      _list.Add(item);
    }

    public Guid Insert(T entity)
    {
      _list.Add(entity);
      return Guid.NewGuid();
    }

    public bool Save(T entity)
    {
      _list.Add(entity);
      return true;
    }

    public void Delete(Guid id)
    {
    }

    public IEnumerable<T> GetAll()
    {
      return _list;
    }

    public T GetById(Guid id)
    {
      return _list.First();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = 2147483647)
    {
      return _list.Where(predicate.Compile()).ToList();
    }

    public bool EnsureIndex<K>(Expression<Func<T, K>> property, bool unique = false)
    {
      throw new NotImplementedException();
    }
  }
}