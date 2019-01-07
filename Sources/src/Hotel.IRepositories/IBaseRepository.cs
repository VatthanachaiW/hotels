using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hotels.IRepositories
{
    //Generic Class ทำให้สามารถรับ Entity ต่างๆ เข้ามาทำงานได้
    public interface IBaseRepository<T> : IDisposable
    {
        //Get ALL
        Task<List<T>> GetAllAsync();

        //Get ALL With Condition
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate);

        //Get By Id
        Task<T> GetByIdAsync(Guid id);

        //Get by Condition
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        //Create new record
        Task AddAsync(T item);

        //Create new record with list
        Task AddRangeAsync(List<T> items);

        //Update record with list
        void Update(T item);

        //Delete record with list
        void Remove(T item);
    }
}