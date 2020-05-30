using MelcoreFramework.Database.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Database.Concrete
{
    public class EntityRepository<Context, EntityType, IdType> : IEntityRepository<EntityType, IdType>
          where IdType : struct
          where EntityType : class, IEntity<IdType>, new()
        where Context : DbContext
    {
        #region Private Fields

        private readonly Context _context;
        private readonly DbSet<EntityType> _entities;
        private readonly Type[] _entityInterfaces;

        #endregion Private Fields



        #region Protected Constructors

        protected EntityRepository(Context context)
        {
            _context = context;
            _entities = context.Set<EntityType>();
            _entityInterfaces = typeof(EntityType).GetInterfaces();
        }

        #endregion Protected Constructors



        #region Public Properties

        public virtual IQueryable<EntityType> Table { get { return _entities.AsQueryable(); } }
        public virtual IQueryable<EntityType> TableAsNoTracking { get { return _entities.AsNoTracking(); } }

        #endregion Public Properties



        #region Public Methods

        public virtual Task Add(EntityType entity, CancellationToken cancelToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return AddTask(entity, cancelToken);
        }

        public virtual Task Add(IEnumerable<EntityType> entity, CancellationToken cancelToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return AddTask(entity, cancelToken);
        }

        public virtual Task Add<InsertType>(InsertType entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where InsertType : IInsert<IdType>, EntityType
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (userId.Equals(default(IdType)))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            return AddTask<InsertType>(entity, userId, cancelToken);
        }

        public virtual Task Add<InsertType>(IEnumerable<InsertType> entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where InsertType : IInsert<IdType>, EntityType
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (userId.Equals(default(IdType)))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return AddTask<InsertType>(entity, userId, cancelToken);
        }

        public virtual Task<bool> Any(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }
            return AnyTask(filter, cancelToken);
        }

        public virtual Task Delete(EntityType entity, CancellationToken cancelToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return DeleteTask(entity, cancelToken);
        }

        public virtual Task Delete(IEnumerable<EntityType> entity, CancellationToken cancelToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return DeleteTask(entity, cancelToken);
        }

        public virtual Task Delete<DeleteType>(DeleteType entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where DeleteType : IDelete<IdType>, EntityType
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (userId.Equals(default(IdType)))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return DeleteTask<DeleteType>(entity, userId, cancelToken);
        }

        public virtual Task Delete<DeleteType>(IEnumerable<DeleteType> entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where DeleteType : IDelete<IdType>, EntityType
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (userId.Equals(default(IdType)))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return DeleteTask<DeleteType>(entity, userId, cancelToken);
        }

        public virtual Task<EntityType> Get(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default(CancellationToken))
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            return GetTask(filter, cancelToken);
        }

        public virtual Task<IEnumerable<EntityType>> GetIEnumerable(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default(CancellationToken))
        {
            return GetIEnumerableTask(filter, cancelToken);
        }

        public virtual Task Update(EntityType entity, CancellationToken cancelToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return UpdateTask(cancelToken);
        }

        public virtual Task Update(IEnumerable<EntityType> entity, CancellationToken cancelToken = default(CancellationToken))
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            return UpdateTask(cancelToken);
        }

        public virtual Task Update<UpdateType>(UpdateType entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where UpdateType : IUpdate<IdType>, EntityType
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (userId.Equals(default(IdType)))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return UpdateTask<UpdateType>(entity, userId, cancelToken);
        }

        public virtual Task Update<UpdateType>(IEnumerable<UpdateType> entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where UpdateType : IUpdate<IdType>, EntityType
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            if (userId.Equals(default(IdType)))
            {
                throw new ArgumentNullException(nameof(userId));
            }
            return UpdateTask<UpdateType>(entity, userId, cancelToken);
        }

        #endregion Public Methods



        #region Private Methods

        private async Task AddTask(EntityType entity, CancellationToken cancelToken = default(CancellationToken))
        {
            await _entities.AddAsync(entity, cancelToken);
            await SaveContext(cancelToken);
        }

        private async Task AddTask(IEnumerable<EntityType> entity, CancellationToken cancelToken = default(CancellationToken))
        {
            await _entities.AddRangeAsync(entity, cancelToken);
            await SaveContext(cancelToken);
        }

        private async Task AddTask<InsertType>(InsertType entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where InsertType : IInsert<IdType>, EntityType
        {
            ((IInsert<IdType>)entity).InsertedBy = userId;
            ((IInsert<IdType>)entity).InsertedAt = DateTime.Now;

            await _entities.AddAsync(entity, cancelToken);
            await SaveContext(cancelToken);
        }

        private async Task AddTask<InsertType>(IEnumerable<InsertType> entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where InsertType : IInsert<IdType>, EntityType
        {
            var now = DateTime.Now;
            Parallel.ForEach(entity, x =>
            {
                ((IInsert<IdType>)x).InsertedBy = userId;
                ((IInsert<IdType>)x).InsertedAt = now;
            });

            await _entities.AddRangeAsync((IEnumerable<EntityType>)entity, cancelToken);
            await SaveContext(cancelToken);
        }

        private Task<bool> AnyTask(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default(CancellationToken))
        {
            return _entities.AnyAsync(filter, cancelToken);
        }

        private async Task DeleteTask(EntityType entity, CancellationToken cancelToken = default(CancellationToken))
        {
            _entities.Remove(entity);
            await SaveContext(cancelToken);
        }

        private async Task DeleteTask(IEnumerable<EntityType> entity, CancellationToken cancelToken = default(CancellationToken))
        {
            _entities.RemoveRange(entity);
            await SaveContext(cancelToken);
        }

        private async Task DeleteTask<DeleteType>(DeleteType entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where DeleteType : IDelete<IdType>, EntityType
        {
            ((IDelete<IdType>)entity).IsDeleted = true;
            ((IDelete<IdType>)entity).DeletedBy = userId;
            ((IDelete<IdType>)entity).DeletedAt = DateTime.Now;
            await SaveContext(cancelToken);
        }

        private async Task DeleteTask<DeleteType>(IEnumerable<DeleteType> entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where DeleteType : IDelete<IdType>, EntityType
        {
            var now = DateTime.Now;
            Parallel.ForEach(entity, x =>
            {
                ((IDelete<IdType>)entity).IsDeleted = true;
                ((IDelete<IdType>)entity).DeletedBy = userId;
                ((IDelete<IdType>)entity).DeletedAt = now;
            });

            await SaveContext(cancelToken);
        }

        private async Task<IEnumerable<EntityType>> GetIEnumerableTask(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default(CancellationToken))
        {
            var q = _entities.AsQueryable();
            if (filter != null)
            {
                q = q.Where(filter);
            }
            if (_entityInterfaces.Any(x => x.Name == nameof(IDelete<IdType>)))
            {
                //todo: !IsDelete eklenecek
            }

            if (_entityInterfaces.Any(x => x.Name == nameof(IActive)))
            {
                //todo: IsActive eklenecek
            }
            return await q.ToListAsync(cancelToken);
        }

        private Task<EntityType> GetTask(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default(CancellationToken))
        {
            return _entities.FirstOrDefaultAsync(filter, cancelToken);
        }

        private Task<int> SaveContext(CancellationToken cancelToken = default)
        {
            if (!cancelToken.IsCancellationRequested)
            {
                return _context.SaveChangesAsync(cancelToken);
            }
            return Task.FromResult(0);
        }

        private async Task UpdateTask(CancellationToken cancelToken = default(CancellationToken))
        {
            await SaveContext(cancelToken);
        }

        private async Task UpdateTask<UpdateType>(UpdateType entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where UpdateType : IUpdate<IdType>, EntityType
        {
            ((IUpdate<IdType>)entity).UpdatedBy = userId;
            ((IUpdate<IdType>)entity).UpdatedAt = DateTime.Now;
            await SaveContext(cancelToken);
        }

        private async Task UpdateTask<UpdateType>(IEnumerable<UpdateType> entity, IdType userId, CancellationToken cancelToken = default(CancellationToken)) where UpdateType : IUpdate<IdType>, EntityType
        {
            var now = DateTime.Now;
            Parallel.ForEach(entity, x =>
            {
                ((IUpdate<IdType>)entity).UpdatedBy = userId;
                ((IUpdate<IdType>)entity).UpdatedAt = now;
            });

            await SaveContext(cancelToken);
        }

        #endregion Private Methods
    }
}