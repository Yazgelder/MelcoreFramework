using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MelcoreFramework.Database.Abstract
{
    public interface IEntityRepository<EntityType, IdType>
        where IdType : struct
        where EntityType : class, IEntity<IdType>, new()
    {
        #region Public Properties

        IQueryable<EntityType> Table { get; }
        IQueryable<EntityType> TableAsNoTracking { get; }

        #endregion Public Properties



        #region Public Methods

        Task Add(EntityType entity, CancellationToken cancelToken = default);

        Task Add(IEnumerable<EntityType> entity, CancellationToken cancelToken = default);

        Task Add<InsertType>(InsertType entity, IdType userId, CancellationToken cancelToken = default) where InsertType : EntityType, IInsert<IdType>;

        Task Add<InsertType>(IEnumerable<InsertType> entity, IdType userId, CancellationToken cancelToken = default) where InsertType : EntityType, IInsert<IdType>;

        Task<bool> Any(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default);

        Task Delete(EntityType entity, CancellationToken cancelToken = default);

        Task Delete(IEnumerable<EntityType> entity, CancellationToken cancelToken = default);

        Task Delete<DeleteType>(DeleteType entity, IdType userId, CancellationToken cancelToken = default) where DeleteType : EntityType, IDelete<IdType>;

        Task Delete<DeleteType>(IEnumerable<DeleteType> entity, IdType userId, CancellationToken cancelToken = default) where DeleteType : EntityType, IDelete<IdType>;

        Task<EntityType> Get(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default);

        Task<IEnumerable<EntityType>> GetIEnumerable(Expression<Func<EntityType, bool>> filter, CancellationToken cancelToken = default);

        Task Update(EntityType entity, CancellationToken cancelToken = default);

        Task Update(IEnumerable<EntityType> entity, CancellationToken cancelToken = default);

        Task Update<UpdateType>(UpdateType entity, IdType userId, CancellationToken cancelToken = default) where UpdateType : EntityType, IUpdate<IdType>;

        Task Update<UpdateType>(IEnumerable<UpdateType> entity, IdType userId, CancellationToken cancelToken = default) where UpdateType : EntityType, IUpdate<IdType>;

        #endregion Public Methods
    }
}