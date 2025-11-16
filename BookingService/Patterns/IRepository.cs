using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

public interface IRepository<T> where T : class
{
    IQueryable<T> GetAll();
    T GetById(int id);
    IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    void Update(T entity);
    void Delete(T entity);
    void DeleteRange(IEnumerable<T> entities);
    void Save();
}