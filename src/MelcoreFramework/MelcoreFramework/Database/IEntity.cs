using System;

namespace MelcoreFramework.Database
{
    public interface IEntity<T> where T : Type
    {
        T Id { get; set; }
    }
}