using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hotels.IDbConnections
{
    public interface IApplicationDbContext : IDisposable
    {
        DatabaseFacade Database { get; }

        DbSet<T> Set<T>() where T : class;

        ChangeTracker ChangeTracker { get; }

        Task<bool> SaveChangesAsync();
    }
}