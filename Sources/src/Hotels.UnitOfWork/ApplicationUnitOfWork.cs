using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Hotels.IDbConnections;
using Hotels.IRepositories;
using Hotels.IUnitOfWorks;
using Hotels.Repositories;
using Ninject;
using Ninject.Parameters;

namespace Hotels.UnitOfWorks
{
    public class ApplicationUnitOfWork : IApplicationUnitOfWork
    {
        private readonly IApplicationDbContext _context;
        private IRoomTypeRepository _roomTypeRepository;

        public ApplicationUnitOfWork(IApplicationDbContext context)
        {
            _context = context;
        }
        /*
        [Obsolete("Not implement method for now")]
        public T Repository<T>() where T : class
        {
            using (var kernel = new StandardKernel())
            {
                kernel.Load(Assembly.GetExecutingAssembly());

                var result = kernel.Get<T>(new ConstructorArgument("context", _context));

                if (result != null && result.GetType().GetInterfaces().Contains(typeof(IBaseRepository<T>)))
                {
                    return result;
                }
            }

            return null;
        }
        */
        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Rollback()
        {
            _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            _disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IRoomTypeRepository RoomTypeRepository =>
            _roomTypeRepository = _roomTypeRepository ?? new RoomTypeRepository(_context);
    }
}