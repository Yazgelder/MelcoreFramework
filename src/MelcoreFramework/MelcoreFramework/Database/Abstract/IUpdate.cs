using System;

namespace MelcoreFramework.Database.Abstract
{
    public interface IUpdate<T> where T : struct
    {
        #region Public Properties

        DateTime? UpdatedAt { get; set; }
        T? UpdatedBy { get; set; }

        #endregion Public Properties
    }
}