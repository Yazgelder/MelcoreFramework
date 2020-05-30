using Microsoft.EntityFrameworkCore;
using System;

namespace MelcoreFramework.Database.Abstract
{
    public interface IEntityContainer
    {
        #region Public Methods

        void AddNamespaceEntity<Context>(Type registerTypeAndSubTypes);

        Type[] GetNamespaceEntity<Context>();

        void RemoveNamespaceEntity<Context>(Type entity);

        void SetEntity<Context>(ModelBuilder modelBuilder);

        #endregion Public Methods
    }
}