using System;

namespace MelcoreFramework.Database.Abstract
{
    public interface IInsert<T> where T : struct
    {
        #region Public Properties

        DateTime InsertedAt { get; set; }
        T InsertedBy { get; set; }

        #endregion Public Properties
    }
}