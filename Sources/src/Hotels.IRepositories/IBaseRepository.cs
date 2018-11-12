using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hotels.IRepositories
{
    public interface IBaseRepository<T> : IDisposable
    {
        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// Get all records with expression
        /// </summary>
        /// <returns></returns>
        List<T> GetAll(Expression<Func<T, bool>> predicate);

        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        T Get(Expression<Func<T, bool>> predicate);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        T GetById(Guid id);
        Task<T> GetByIdAsync(Guid id);

        void Add(T item);
        Task AddAsync(T item);
        void AddRange(List<T> items);
        Task AddRangeAsync(List<T> items);

        void Update(T item);
        void UpdateRange(List<T> items);

        void Remove(T item);
        void RemoveRange(List<T> items);
    }
}