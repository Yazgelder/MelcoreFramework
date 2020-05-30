using MelcoreFramework.Database.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MelcoreFramework.Database.Concrete
{
    public class EntityContainer : IEntityContainer
    {
        #region Private Fields

        private readonly object _lockKey = new object();
        private Dictionary<Type, List<Type>> _container;

        #endregion Private Fields

        #region Public Constructors

        public EntityContainer()
        {
        }

        #endregion Public Constructors



        #region Public Methods

        public void AddNamespaceEntity<Context>(Type registerTypeAndSubTypes)
        {
            Type contextType = typeof(Context);
            Type entityBase = typeof(IEntity<>);
            var list = registerTypeAndSubTypes.Assembly.GetExportedTypes();
            string namespaceName = registerTypeAndSubTypes.Namespace;
            lock (_lockKey)
            {
                if (_container == null)
                    _container = new Dictionary<Type, List<Type>>();

                if (!_container.ContainsKey(contextType))
                    _container.Add(contextType, new List<Type>());
            }
            _container[contextType].AddRange(list.Where(x => CheckEntity(x, entityBase, namespaceName)));
        }

        public Type[] GetNamespaceEntity<Context>()
        {
            Type contextType = typeof(Context);
            return _container[contextType].ToArray();
        }

        public void RemoveNamespaceEntity<Context>(Type entity)
        {
            Type contextType = typeof(Context);
            if (!_container[contextType].Contains(entity))
            {
                throw new KeyNotFoundException();
            }
            _container[contextType].Remove(entity);
        }

        public void SetEntity<Context>(ModelBuilder modelBuilder)
        {
            Type contextType = typeof(Context);
            foreach (var item in _container[contextType])
            {
                modelBuilder.Entity(item);
            }
        }

        #endregion Public Methods



        #region Private Methods

        private bool CheckEntity(Type entity, Type entityBase, string namespaceName)
        {
            var start = entity.Namespace.StartsWith(namespaceName);

            var q = entity.GetInterfaces().Any(y => y.Name == entityBase.Name && y.Assembly == entityBase.Assembly);

            return q && start;
        }

        #endregion Private Methods
    }
}