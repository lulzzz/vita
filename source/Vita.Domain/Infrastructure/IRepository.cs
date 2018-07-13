using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Vita.Domain.Infrastructure
{
  public interface IRepository<T>
  {
    Guid Insert(T entity);
    bool Save(T entity);
    void Delete(Guid id);
    IEnumerable<T> GetAll();
    T GetById(Guid id);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate, int skip = 0, int limit = 2147483647);
    bool EnsureIndex<K>(Expression<Func<T, K>> property, bool unique = false);
  }
}