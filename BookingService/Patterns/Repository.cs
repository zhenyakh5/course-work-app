using BookingService;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> GetAll() => _context.Set<T>();

    public T GetById(int id) => _context.Set<T>().Find(id);

    public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
    {
        return _context.Set<T>().Where(predicate).ToList();
    }

    public void Add(T entity) => _context.Set<T>().Add(entity);

    public void AddRange(IEnumerable<T> entities) => _context.Set<T>().AddRange(entities);

    public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

    public void Delete(T entity) => _context.Set<T>().Remove(entity);

    public void DeleteRange(IEnumerable<T> entities) => _context.Set<T>().RemoveRange(entities);

    public void Save() => _context.SaveChanges();
}