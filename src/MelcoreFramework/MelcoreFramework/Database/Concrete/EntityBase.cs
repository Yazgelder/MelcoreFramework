using MelcoreFramework.Database.Abstract;

namespace MelcoreFramework.Database.Concrete
{
    public abstract class EntityBase<T> : IEntity<T> where T : struct
    {
        #region Public Properties

        public T Id { get; set; }

        #endregion Public Properties
    }
}