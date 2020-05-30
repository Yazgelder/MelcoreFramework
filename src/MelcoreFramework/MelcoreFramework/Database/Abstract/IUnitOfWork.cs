using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Database.Abstract
{
    public interface IUnitOfWork<T> : IDisposable
        where T : DbContext
    {
        #region Public Methods

        Task BeginTransaction(CancellationToken cancelToken = default(CancellationToken));

        Task Commit(CancellationToken cancelToken = default(CancellationToken));

        Task Rollback(CancellationToken cancelToken = default(CancellationToken));

        #endregion Public Methods
    }
}