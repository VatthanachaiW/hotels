using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hotels.Entities;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Hotels.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private bool _disposed = false;

        protected readonly IApplicationDbContext _context;

        public BaseRepository(IApplicationDbContext context)
        {
            _context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this._disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().SingleOrDefault(predicate);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(predicate);
        }

        public T GetById(Guid id)
        {
            return _context.Set<T>().Find(id);
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public void Add(T item)
        {
            _context.Set<T>().Add(item);
        }

        public async Task AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public void AddRange(List<T> items)
        {
            _context.Set<T>().AddRange(items);
        }

        public async Task AddRangeAsync(List<T> items)
        {
            await _context.Set<T>().AddRangeAsync(items);
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }

        public void UpdateRange(List<T> items)
        {
            _context.Set<T>().UpdateRange(items);
        }

        public void Remove(T item)
        {
            _context.Set<T>().Remove(item);
        }

        public void RemoveRange(List<T> items)
        {
            _context.Set<T>().RemoveRange(items);
        }
    }
}