using MelcoreFramework.Database.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Database.Concrete
{
    public class UnitOfWork<T> : IUnitOfWork<T>
         where T : DbContext
    {
        #region Private Fields

        private readonly T _context;
        private bool disposedValue;
        private IDbContextTransaction transaction = null;

        #endregion Private Fields



        #region Protected Constructors

        protected UnitOfWork(T context)
        {
            _context = context;
        }

        #endregion Protected Constructors



        #region Public Methods

        public async Task BeginTransaction(CancellationToken cancelToken = default(CancellationToken))
        {
            transaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted, cancelToken);
        }

        public Task Commit(CancellationToken cancelToken = default(CancellationToken))
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("Please run the BeginTransaction first.");
            }

            return CommitTask(cancelToken);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public Task Rollback(CancellationToken cancelToken = default(CancellationToken))
        {
            if (transaction == null)
            {
                throw new InvalidOperationException("Please run the BeginTransaction first.");
            }

            return RollbackTask(cancelToken);
        }

        #endregion Public Methods



        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && transaction != null)
                {
                    transaction.Dispose();
                }
                transaction = null;
                disposedValue = true;
            }
        }

        #endregion Protected Methods

        #region Private Methods

        private async Task CommitTask(CancellationToken cancelToken = default(CancellationToken))
        {
            await transaction.CommitAsync(cancelToken);
            transaction.Dispose();
            transaction = null;
        }

        private async Task RollbackTask(CancellationToken cancelToken = default(CancellationToken))
        {
            await transaction.RollbackAsync(cancelToken);
            transaction.Dispose();
            transaction = null;
        }

        #endregion Private Methods
    }
}