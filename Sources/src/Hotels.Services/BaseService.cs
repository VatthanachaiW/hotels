using System;
using AutoMapper;
using Hotels.IUnitOfWorks;
using Microsoft.Extensions.Logging;

namespace Hotels.Services
{
    public class BaseService
    {
        private bool _disposed = false;
        protected readonly IApplicationUnitOfWork UnitOfWork;
        protected ILogger<BaseService> Logger;
        protected MapperConfiguration MapperConfiguration;

        public BaseService(IApplicationUnitOfWork unitOfWork, ILogger<BaseService> logger)
        {
            Logger = logger;
            UnitOfWork = unitOfWork;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    UnitOfWork.Dispose();
                }
            }

            this._disposed = true;
        }

        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}