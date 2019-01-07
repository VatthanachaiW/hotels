using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hotel.IRepositories;
using Hotels.Entities.Masters;
using Hotels.IDbConnections;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity //กำหนดว่าจะใช้งานเฉพาะ class ที่ Implement มาจาก BaseEntity เท่านั้น
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

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task AddAsync(T item)
        {
            await _context.Set<T>().AddAsync(item);
        }

        public async Task AddRangeAsync(List<T> items)
        {
            await _context.Set<T>().AddRangeAsync(items);
        }

        public void Update(T item)
        {
            _context.Set<T>().Update(item);
        }

        public void Remove(T item)
        {
            _context.Set<T>().Remove(item);
        }
    }
}