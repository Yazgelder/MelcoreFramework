using System;

namespace MercoreFramework.Databese
{
    public interface IEntity<T> where T : Type
    {
        T Id { get; set; }
    }
}