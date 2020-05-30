using System;

namespace MelcoreFramework.Database.Abstract
{
    public interface IDelete<T> where T : struct
    {
        #region Public Properties

        DateTime? DeletedAt { get; set; }
        T? DeletedBy { get; set; }
        bool IsDeleted { get; set; }

        #endregion Public Properties
    }
}