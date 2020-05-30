namespace MelcoreFramework.Database.Abstract
{
    public interface IEntity<T> where T : struct
    {
        #region Public Properties

        T Id { get; set; }

        #endregion Public Properties
    }
}