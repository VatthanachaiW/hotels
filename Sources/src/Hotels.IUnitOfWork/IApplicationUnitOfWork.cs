using System;
using System.Threading.Tasks;
using Hotels.IRepositories;

namespace Hotels.IUnitOfWorks
{
    public interface IApplicationUnitOfWork : IDisposable
    {
        Task<bool> CommitAsync();

        void Rollback();
        /*
        [Obsolete("Not implement method for now")]
        T Repository<T>() where T : class;
        */
        IRoomTypeRepository RoomTypeRepository { get; }
    }
}